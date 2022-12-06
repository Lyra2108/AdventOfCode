var communication = Communication();

const int chunkLength = 14;

for (var i = chunkLength; i < communication.Length; i++)
    if (communication[(i - chunkLength)..i].Distinct().Count() == chunkLength)
    {
        Console.WriteLine($"The start frame ends at {i}");
        break;
    }

static string Communication()
{
    using var fileReader = new StreamReader("input.txt");
    return fileReader.ReadLine()!;
}