var signals = Signals().ToArray();
var indexSum = signals
    .Select((signal, i) => (index: i + 1, signal))
    .Where((pair) => pair.signal.left.CompareTo(pair.signal.right) < 0)
    .Sum(pair => pair.index);

Console.WriteLine($"The signal index sum is: {indexSum}");

var firstDistress = new NestedList { Items = { new NestedList { Items = { 2 } } } };
var secondDistress = new NestedList { Items = { new NestedList { Items = { 6 } } } };
var allSignals = signals
    .SelectMany(signal => new[] { signal.left, signal.right })
    .Concat(new[]
    {
        firstDistress,
        secondDistress,
    })
    .Order()
    .ToList();

var firstIndex = allSignals.IndexOf(firstDistress) + 1;
var secondIndex = allSignals.IndexOf(secondDistress) + 1;

Console.WriteLine($"The decoder key for the distress signal is: {firstIndex * secondIndex}");
    
IEnumerable<(NestedList left, NestedList right)> Signals()
{
    using var reader = new StreamReader("input.txt");
    while (reader.ReadLine() is { } leftLine)
    {
        yield return (
            left: ParseLine(leftLine),
            right: ParseLine(reader.ReadLine()!)
        );

        reader.ReadLine();
    }
}


NestedList ParseLine(ReadOnlySpan<char> line)
{
    var root = new NestedList();
    var current = root;
    var numberStart = (int?)null;
    for (var i = 0; i < line.Length; i++)
        switch (line[i])
        {
            case '[':
                var child = new NestedList { Parent = current };
                current.Items.Add(child);
                current = child;
                break;
            case ']':
                if (numberStart is not null)
                {
                    current.Items.Add(int.Parse(line[numberStart.Value..i]));
                    numberStart = null;
                }

                current = current.Parent!;
                break;
            case ',':
                if (numberStart is null)
                    break;

                current.Items.Add(int.Parse(line[numberStart.Value..i]));
                numberStart = null;
                break;
            default:
                numberStart ??= i;
                break;
        }


    return root;
}

internal class NestedList : IComparable<NestedList>
{
    public NestedList? Parent { get; set; }
    public IList<object> Items { get; } = new List<object>();

    public int CompareTo(NestedList? other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        if (!Items.Any() && other.Items.Any())
            return -1;
        if (Items.Any() && !other.Items.Any())
            return 1;

        foreach (var (item, i) in Items.Select((item, i) => (item, i)))
        {
            if (i > other.Items.Count - 1)
                return 1;

            var otherItem = other.Items[i];
            switch (item)
            {
                case int number when otherItem is NestedList otherList:
                {
                    var compare = new NestedList { Items = { number } }.CompareTo(otherList);
                    if (compare != 0)
                        return compare;
                    break;
                }
                case NestedList list when otherItem is int otherNumber:
                {
                    var compare = list.CompareTo(new NestedList { Items = { otherNumber } });
                    if (compare != 0)
                        return compare;
                    break;
                }
                case int number when otherItem is int otherNumber:
                {
                    var compare = number.CompareTo(otherNumber);
                    if (compare != 0)
                        return compare;
                    break;
                }
                case NestedList list when otherItem is NestedList otherList:
                {
                    var compare = list.CompareTo(otherList);
                    if (compare != 0)
                        return compare;
                    break;
                }
            }
        }

        if (Items.Count < other.Items.Count)
            return -1;

        return 0;
    }
}