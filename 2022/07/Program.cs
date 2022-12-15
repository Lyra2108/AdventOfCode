using System.Text.RegularExpressions;

var rootFolder = Folders();

CalculateSize(rootFolder);
var allFolders = GetAllFolders(rootFolder).ToArray();

var sumOfSmallerThan100000 = allFolders
    .Where(folder => folder.Size <= 100000)
    .Sum(folder => folder.Size);
Console.WriteLine($"The sum of all folders smaller then 100000 is {sumOfSmallerThan100000}");

var neededSpace = 30000000 - (70000000 - rootFolder.Size!);
var smallestFolder = allFolders
    .Select(folder => folder.Size)
    .Where(size => size >= neededSpace)
    .Min();
Console.WriteLine($"The smallest needed folder has the size {smallestFolder}");

void CalculateSize(Folder folder)
{
    var unknownChildren = folder.Children.Values.OfType<Folder>().Where(f => f.Size == null);
    foreach (var unknownChild in unknownChildren)
        CalculateSize(unknownChild);

    folder.Size = folder.Children.Values.Sum(file => file.Size);
}

IEnumerable<Folder> GetAllFolders(Folder folder)
{
    yield return folder;
    foreach (var childFolder in folder.Children.Values.OfType<Folder>())
    {
        var childsFolders = GetAllFolders(childFolder);
        foreach (var childsFolder in childsFolders)
            yield return childsFolder;
    }
}

static Folder Folders()
{
    var cdRegex = CdRegex();
    var lsRegex = LsRegex();
    var fileLine = FileRegex();
    var root = new Folder { Name = "/" };
    using var fileReader = new StreamReader("input.txt");
    var currentFolder = root;
    while (fileReader.ReadLine() is { } line)
    {
        var cdMatch = cdRegex.Match(line);
        if (cdMatch.Success)
        {
            var folderName = cdMatch.Groups["name"].Value;
            switch (folderName)
            {
                case "/":
                    currentFolder = root;
                    break;
                case "..":
                    currentFolder = currentFolder.Parent!;
                    break;
                default:
                    currentFolder = (Folder)currentFolder.Children[folderName];
                    continue;
            }
        }

        var lsMatch = lsRegex.Match(line);
        if (lsMatch.Success) continue;

        var fileMatch = fileLine.Match(line);
        if (fileMatch.Success)
        {
            if (fileMatch.Groups["folder"].Success)
            {
                var folder = new Folder
                {
                    Name = fileMatch.Groups["folderName"].Value,
                    Parent = currentFolder
                };
                currentFolder.Children[folder.Name] = folder;
            }
            else
            {
                var file = new File
                {
                    Name = fileMatch.Groups["fileName"].Value,
                    Parent = currentFolder,
                    Size = int.Parse(fileMatch.Groups["size"].Value)
                };
                currentFolder.Children[file.Name] = file;
            }
        }
    }

    return root;
}

public interface IFile
{
    public Folder? Parent { get; init; }
    public string Name { get; init; }
    public int? Size { get; set; }
}

public class File : IFile
{
    public Folder? Parent { get; init; }
    public required string Name { get; init; }
    public int? Size { get; set; }
}

public class Folder : File
{
    public IDictionary<string, IFile> Children { get; } = new Dictionary<string, IFile>();
}

internal partial class Program
{
    [GeneratedRegex("\\$ ls", RegexOptions.Compiled)]
    private static partial Regex LsRegex();

    [GeneratedRegex("\\$ cd (?<name>.+)", RegexOptions.Compiled)]
    private static partial Regex CdRegex();

    [GeneratedRegex("((?<folder>dir (?<folderName>.+))|(?<file>(?<size>\\d+) (?<fileName>.+)))")]
    private static partial Regex FileRegex();
}