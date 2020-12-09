using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

var rules = ReadInLines().Select(line => new Rule(line)).ToDictionary(rule => rule.MainColor);

Console.WriteLine($"{rules.Values.Count(rule => rule.CanContain("shiny gold", rules))} colors can contain a shiny gold bag.");
Console.WriteLine($"A shiny gold bag contains {rules["shiny gold"].ContainsBags(rules)} bags.");

static IEnumerable<string> ReadInLines()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while ((line = fileReader.ReadLine()) != null)
    {
        yield return line;
    }
}

class Rule
{
    private static readonly Regex _ruleRegex = new Regex(@"(?'mainColor'\w+ \w+) bags contain((?'nothing' no other bags)|(?'otherBag' (?'times'\d+) (?'otherColor'\w+ \w+) bags?,?)+)\.");

    public string MainColor { get; }
    public IDictionary<string, int> InnerBags { get; }
    public Rule(string line)
    {
        var match = _ruleRegex.Match(line);

        MainColor = match.Groups["mainColor"].Value;
        if (match.Groups["nothing"].Success)
            InnerBags = null;
        else
        {
            InnerBags = match.Groups["otherBag"].Captures
                .Select((capture, i) => new KeyValuePair<string, int>(
                    match.Groups["otherColor"].Captures[i].Value,
                    int.Parse(match.Groups["times"].Captures[i].Value
                )))
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }

    public bool CanContain(string color, IDictionary<string, Rule> otherRules)
    {
        if (InnerBags == null)
            return false;
        if (InnerBags.Keys.Contains(color))
            return true;

        return InnerBags.Keys.Any(key => otherRules[key].CanContain(color, otherRules));
    }

    public int ContainsBags(IDictionary<string, Rule> otherRules)
    {
        if (InnerBags == null)
            return 0;

        return InnerBags.Sum(pair => pair.Value + pair.Value * otherRules[pair.Key].ContainsBags(otherRules));
    }
}
