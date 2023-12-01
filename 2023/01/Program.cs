using System.Text.RegularExpressions;

var inputNumbers = InputNumbers().ToList();
var sum = inputNumbers.Sum();

Console.WriteLine($"The sum is {sum}.");

var sumWithStrings = InputNumbersWithStrings().Sum();

Console.WriteLine($"The sum with strings is {sumWithStrings}.");

static IEnumerable<int> InputNumbers()
{
    var regex = new Regex(@".*?(?'first'\d)(.*(?'last'\d).*?|.*)");
    using var fileReader = new StreamReader("input.txt");

    while (fileReader.ReadLine() is { } line)
    {
        var match = regex.Match(line);
        var first = match.Groups["first"].Value;
        var last = match.Groups["last"].Success ? match.Groups["last"].Value : first;
        yield return int.Parse(first + last);
    }
}

static IEnumerable<int> InputNumbersWithStrings()
{
    var regex = new Regex(
        @".*?(?'first'\d|one|two|three|four|five|six|seven|eight|nine)(.*(?'last'\d|one|two|three|four|five|six|seven|eight|nine).*?|.*)");
    using var fileReader = new StreamReader("input.txt");

    while (fileReader.ReadLine() is { } line)
    {
        var match = regex.Match(line);
        var first = match.Groups["first"].Value;
        var last = match.Groups["last"].Success ? match.Groups["last"].Value : first;
        yield return int.Parse(Format(first) + Format(last));
    }
}

static string Format(string input)
{
    return input switch
    {
        "one" => "1",
        "two" => "2",
        "three" => "3",
        "four" => "4",
        "five" => "5",
        "six" => "6",
        "seven" => "7",
        "eight" => "8",
        "nine" => "9",
        _ => input
    };
}