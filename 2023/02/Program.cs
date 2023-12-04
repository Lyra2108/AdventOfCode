using System.Text.RegularExpressions;
using Display = (int Blue, int Green, int Red);

var games = ReadInGames().ToList();

var sum = games
    .Where(game => game.Displays.All(show => show is { Red: <= 12, Blue: <= 14, Green: <= 13 }))
    .Select(game => game.Id)
    .Sum();

Console.WriteLine($"The sum is {sum}");

var power = games
    .Select(MinColors)
    .Select(min => min.Blue * min.Red * min.Green)
    .Sum();

Console.WriteLine($"The power is {power}");

static IEnumerable<Game> ReadInGames()
{
    var showString = @"( ((?'blue'\d+) blue|(?'red'\d+) red|(?'green'\d+) green),?)+";
    var regex = new Regex(
        $@"^Game (?'id'\d+):(?'show'{showString};?)+$");
    var showRegex = new Regex(showString);
    using var fileReader = new StreamReader("input.txt");

    while (fileReader.ReadLine() is { } line)
    {
        var match = regex.Match(line);
        var id = int.Parse(match.Groups["id"].Value);
        var shown = new HashSet<Display>();
        foreach (Capture capture in match.Groups["show"].Captures)
        {
            var showMatch = showRegex.Match(capture.Value);
            var red = showMatch.Groups["red"].Success ? int.Parse(showMatch.Groups["red"].Value) : 0;
            var blue = showMatch.Groups["blue"].Success ? int.Parse(showMatch.Groups["blue"].Value) : 0;
            var green = showMatch.Groups["green"].Success ? int.Parse(showMatch.Groups["green"].Value) : 0;
            shown.Add((blue, green, red));
        }
        yield return new Game(id, shown);
    }
}

Display MinColors(Game game)
{
    var shown = game.Displays;
    var blue = shown.Select(x => x.Blue).Max();
    var red = shown.Select(x => x.Red).Max();
    var green = shown.Select(x => x.Green).Max();
    return (blue, green, red);
}

public record Game(int Id, ISet<Display> Displays);
