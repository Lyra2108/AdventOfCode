var priorities = InputLines()
    .Select(line =>
    {
        var middle = line.Length / 2;
        return (first: line[..middle], second: line[middle..]);
    })
    .Select(compartments => compartments.first.Intersect(compartments.second).First())
    .Select(ScoreType)
    .Sum();

Console.WriteLine($"The sum of the priorities is {priorities}");

var groups = InputLines()
    .Chunk(3)
    .Select(group => group[0].Intersect(group[1]).Intersect(group[2]).First())
    .Select(ScoreType)
    .Sum();

Console.WriteLine($"The groups score is {groups}");

int ScoreType(char type)
{
    if (type is >= 'a' and <= 'z')
        return type - 'a' + 1;
    return type - 'A' + 27;
}

static IEnumerable<string> InputLines()
{
    using var fileReader = new StreamReader("input.txt");
    string? line;

    while ((line = fileReader.ReadLine()) != null)
    {
        yield return line;
    }
}
