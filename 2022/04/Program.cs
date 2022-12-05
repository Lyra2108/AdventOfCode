using System.Text.RegularExpressions;

var overlappingPairs = InputRanges()
    .Select(pairs =>
    {
        var first = pairs.First();
        var second = pairs.Last();

        if ((first.start >= second.start && first.end <= second.end)
            || (second.start >= first.start && second.end <= first.end))
            return 1;

        return 0;
    })
    .Sum();

Console.WriteLine($"There are {overlappingPairs} overlapping pairs");

var intersecting = InputRanges()
    .Select(pairs =>
    {
        var first = pairs.First();
        var second = pairs.Last();

        if (
            !(
                first.start > second.end
                || first.end < second.start
            )
        )
            return 1;

        return 0;
    })
    .Sum();

Console.WriteLine($"There are {intersecting} intersecting pairs");

static IEnumerable<(int start, int end)[]> InputRanges()
{
    var regex = new Regex(@"(?'start1'\d+)-(?'end1'\d+),(?'start2'\d+)-(?'end2'\d+)");
    using var fileReader = new StreamReader("input.txt");

    while (fileReader.ReadLine() is { } line)
    {
        var match = regex.Match(line);
        yield return new[]
        {
            (start: int.Parse(match.Groups["start1"].Value), end: int.Parse(match.Groups["end1"].Value)),
            (start: int.Parse(match.Groups["start2"].Value), end: int.Parse(match.Groups["end2"].Value))
        };
    }
}