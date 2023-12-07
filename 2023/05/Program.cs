using System.Text.RegularExpressions;
using Range = (long start, long length);

var (seeds, maps) = ReadIn();

var location = seeds.Select(seed =>
    {
        foreach (var map in maps)
            seed = map.Transition(seed);
        return seed;
    })
    .Min();

Console.WriteLine($"The location is {location}");


var ranges = seeds
    .Select((seed, i) => (seed, i))
    .GroupBy(tuple => tuple.i/2)
    .Select(tuples =>
    {
        var valueTuples = tuples.ToArray();
        return (start: valueTuples[0].seed, length: valueTuples[1].seed);
    })
    .ToList();

var temp = new List<Range>();
foreach (var map in maps)
{
    foreach (var range in ranges)
        temp.AddRange(map.Transition(range));

    ranges = temp;
    temp = new List<Range>();
}

var location2 = ranges
    .Select(range => range.start).Min();

Console.WriteLine($"The location is {location2 -1}");
return;

(ICollection<long> seeds, ICollection<Map> maps) ReadIn()
{
    var numberRegex = new Regex(@"(?'number'\d+)");
    using var fileReader = new StreamReader("input.txt");
    var seeds = numberRegex.Matches(fileReader.ReadLine()!)
        .Select(match => long.Parse(match.Value))
        .ToList();

    var maps = new List<Map>();
    Map? currentMap = null;
    while (fileReader.ReadLine() is { } line)
    {
        if (line == string.Empty)
        {
            if (currentMap is not null)
                maps.Add(currentMap);
            continue;
        }

        if (line.Contains("map"))
        {
            currentMap = new Map();
            continue;
        }

        currentMap!.Add(numberRegex.Matches(line)
            .Select(match => long.Parse(match.Value))
            .ToList());
    }

    return (seeds, maps);
}


public class Map
{
    private readonly Dictionary<long, (long source, long destination, long length)> _ranges;
    private readonly SortedSet<long?> _sources;

    public Map()
    {
        _sources = new SortedSet<long?>();
        _ranges = new Dictionary<long, (long source, long destination, long length)>();
    }

    public void Add(List<long> numbers)
    {
        var (source, destination, length) = (numbers[1], numbers[0], numbers[2]);

        _sources.Add(source);
        _ranges[source] = (source, destination, length);
    }

    public long Transition(long seed)
    {
        var source = _sources.LastOrDefault(source => source <= seed);
        if (source is null)
            return seed;

        var range = _ranges[source.Value];
        var distance = seed - source.Value;
        return range.length >= distance ? range.destination + distance : seed;
    }

    public IEnumerable<Range> Transition(Range range)
    {
        var source = _sources.LastOrDefault(source => source <= range.start);
        if (source is null)
            return new[] { range };

        var transitionRange = _ranges[source.Value];
        var transitionRangeEnd = transitionRange.source + transitionRange.length;
        if (transitionRangeEnd < range.start)
            return new[] { range };

        var rangeEnd = range.start + range.length;
        if (transitionRangeEnd >= rangeEnd)
            return new[] { (transitionRange.destination + (range.start - transitionRange.source), range.length) };

        var tooLong = rangeEnd - transitionRangeEnd;
        var fits = range.length - tooLong;
        return new List<Range> { (transitionRange.destination + (range.start - transitionRange.source), fits) }
            .Concat(Transition((range.start + fits + 1, tooLong)));
    }
}