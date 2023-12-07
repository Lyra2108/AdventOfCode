// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var cards = ReadInCards().ToList();
var totalPoints = cards
    .Select(card => card.Numbers.Intersect(card.Winners).Count())
    .Select(hit => hit == 0 ? 0 : 1 << (hit - 1))
    .Sum();

Console.WriteLine($"The total points are {totalPoints}");

var winnings = cards.ToDictionary(card => card.Id, card => new Win(
card.Numbers.Intersect(card.Winners).Count()));
foreach (var (id, card) in winnings)
    for (var i = id + 1; i <= id + card.Winnings; i++)
        winnings[i].Counts += card.Counts;

var totalCards = winnings.Values.Select(win => win.Counts).Sum();
Console.WriteLine($"There are {totalCards} cards");
return;

IEnumerable<Card> ReadInCards()
{
    using var fileReader = new StreamReader("input.txt");
    while (fileReader.ReadLine() is { } line)
    {
        var cardMatch = CardRegex().Match(line);
        yield return new Card
        (
            int.Parse(cardMatch.Groups["id"].Value),
            cardMatch.Groups["number"].Captures.Select(capture => int.Parse(capture.Value)).ToList(),
            cardMatch.Groups["winner"].Captures.Select(capture => int.Parse(capture.Value)).ToList()
        );
    }
}

public class Win(int winnings)
{
    public int Winnings { get; } = winnings;
    public int Counts { get; set; } = 1;
}
public record Card(int Id, ICollection<int> Numbers, ICollection<int> Winners);

internal partial class Program
{
    [GeneratedRegex(@"^Card\s+(?'id'\d+):(\s+(?'number'\d+))+\s+\|(\s+(?'winner'\d+))+$")]
    private static partial Regex CardRegex();
}
