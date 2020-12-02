using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

var input = ReadInLines();
var regex = new Regex(@"(?'min'\d+)-(?'max'\d+) (?'char'\w): (?'password'\w+)");

var validPasswords = input.Count(line =>
{
    var match = regex.Match(line);
    var rule = new TimesPasswordPolicy(ParseInt(match, "min"), ParseInt(match, "max"), match.Groups["char"].Value.ToCharArray()[0]);

    return rule.IsValid(match.Groups["password"].Value);
});

Console.WriteLine($"There are {validPasswords} with the times policy.");

var valid2ndRulePasswords = input.Count(line =>
{
    var match = regex.Match(line);
    var rule = new PositionPasswordPolicy(ParseInt(match, "min"), ParseInt(match, "max"), match.Groups["char"].Value.ToCharArray()[0]);

    return rule.IsValid(match.Groups["password"].Value);
});

Console.WriteLine($"There are {valid2ndRulePasswords} with the position policy.");

static IEnumerable<string> ReadInLines()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while((line = fileReader.ReadLine()) != null){
        yield return line;
    }
}

static int ParseInt(Match match, string name)
{
    return int.Parse(match.Groups[name].Value);
}

class TimesPasswordPolicy {
    private int _minOccurences;
    private int _maxOccurences;

    private char _requieredChar;

    public TimesPasswordPolicy(int minOccurences, int maxOccurences, char requieredChar)
    {
        _minOccurences = minOccurences;
        _maxOccurences = maxOccurences;
        _requieredChar = requieredChar;
    }

    public virtual bool IsValid(string password){
        var occurences = password.ToCharArray().Count(letter => letter == _requieredChar);
        return occurences >= _minOccurences && occurences <= _maxOccurences;
    }
}

class PositionPasswordPolicy {
    private int _firstPosition;
    private int _secondPosition;

    private char _requieredChar;

    public PositionPasswordPolicy(int firstPosition, int secondPosition, char requieredChar)
    {
        _firstPosition = --firstPosition;
        _secondPosition = --secondPosition;
        _requieredChar = requieredChar;
    }

    public virtual bool IsValid(string password)
    {
        var passwordArray = password.ToCharArray();
        return (HasCharAtPosition(_firstPosition)) ^ HasCharAtPosition(_secondPosition);

        bool HasCharAtPosition(int position)
        {
            return passwordArray.Length > position && passwordArray[position] == _requieredChar;
        }
    }
}
