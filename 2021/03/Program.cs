var debugCodes = DebugCodes().ToArray();
var inputMajority = debugCodes.Length / 2d;
var debugCodeLength = 12;

int[] bitCounts = new int[debugCodeLength];
foreach (var debugCode in debugCodes)
{
    for (int i = 0; i < debugCodeLength; i++)
    {
        bitCounts[i] += BitAtPosition(debugCode, i);
    }
}

var gammaRate = 0;
for (int i = 0; i < debugCodeLength; i++)
    gammaRate |= bitCounts[i] >= inputMajority ? 0 : 1 << i;

var epsilon = ~gammaRate << (32 - debugCodeLength) >> (32 - debugCodeLength);

Console.WriteLine($"The power consumption of the submarine is {gammaRate * epsilon}");

var oxygen = GetReading(debugCodes, (bitCount, mayority) => bitCount >= mayority, debugCodeLength);
var co2 = GetReading(debugCodes, (bitCount, mayority) => bitCount < mayority, debugCodeLength);

Console.WriteLine($"The life support rating of the submarine is {oxygen * co2}");


static int BitAtPosition(short number, int i)
{
    return number >> i & 1;
}

static int GetReading(short[] numbers, Func<int, double, bool> criteria, int debugCodeLength)
{
    for (int i = debugCodeLength - 1; i >= 0; i--)
    {
        var bitCountForPosition = numbers.Sum(number => BitAtPosition(number, i));
        var mayority = numbers.Length / 2d;
        var neededBit = criteria(bitCountForPosition, mayority) ? 0 : 1;
        numbers = numbers.Where(number => BitAtPosition(number, i) == neededBit).ToArray();

        if (numbers.Length == 1)
            return numbers.First();
    }
    throw new Exception("There is a bug above");
}

static IEnumerable<short> DebugCodes()
{
    using var fileReader = new StreamReader("input.txt");
    string? line;
    while ((line = fileReader.ReadLine()) != null)
    {
        yield return Convert.ToInt16(line, 2);
    }
}
