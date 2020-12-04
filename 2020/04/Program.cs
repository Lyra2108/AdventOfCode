using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

var requieredFields = new HashSet<string> { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
var heightRegex = new Regex(@"^(?'heightNumber'\d+)(?'heightMetric'(cm|in))$");
var hairRegex = new Regex(@"^#[a-f0-9]{6}$");
var eyeRegex = new Regex(@"^(amb|blu|brn|gry|grn|hzl|oth)$");
var passportIdRegex = new Regex(@"^\d{9}$");

var passports = ReadInPassports();

Console.WriteLine($"There are {passports.Count(IsValidPassportAfter1Rule)} valid passports after 1. Rule.");
Console.WriteLine($"There are {passports.Count(IsValidPassportAfter2Rule)} valid passports after 2. Rule.");

bool IsValidPassportAfter1Rule(IDictionary<string, string> passport)
{
    return !requieredFields.Except(passport.Keys).Any();
}

bool IsValidPassportAfter2Rule(IDictionary<string, string> passport)
{
    if (requieredFields.Except(passport.Keys).Any())
        return false;

    var birthYear = int.Parse(passport["byr"]);
    if (birthYear > 2002 || birthYear < 1920)
        return false;

    var issueYear = int.Parse(passport["iyr"]);
    if (issueYear > 2020 || issueYear < 2010)
        return false;

    var expireYear = int.Parse(passport["eyr"]);
    if (expireYear > 2030 || expireYear < 2020)
        return false;

    if (!eyeRegex.IsMatch(passport["ecl"])
        || !hairRegex.IsMatch(passport["hcl"])
        || !passportIdRegex.IsMatch(passport["pid"])
        || !heightRegex.IsMatch(passport["hgt"]))
        return false;

    var heightMatch = heightRegex.Match(passport["hgt"]);
    var height = int.Parse(heightMatch.Groups["heightNumber"].Value);
    var heightMetric = heightMatch.Groups["heightMetric"].Value;

    return (heightMetric == "cm" && height >= 150 && height <= 193) || (heightMetric == "in" && height >= 59 && height <= 76);
}

static IEnumerable<IDictionary<string, string>> ReadInPassports()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    var passportDict = new Dictionary<string, string>();
    while ((line = fileReader.ReadLine()) != null)
    {
        if (line == string.Empty)
        {
            var temp = passportDict;
            passportDict = new Dictionary<string, string>();
            yield return temp;
        }
        else
        {
            var lineDict = line.Split(" ").Select(pair => pair.Split(":")).ToDictionary(pair => pair[0], pair => pair[1]);
            passportDict = passportDict.Concat(lineDict).ToDictionary(x => x.Key, x => x.Value);
        }

    }
    yield return passportDict;
}
