using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

var encryptedNumbers = ReadInNumbers().ToArray();

var invalidNumber = encryptedNumbers
    .Skip(25)
    .Where((number, i) =>
    {
        var preamble = encryptedNumbers[i..(i + 25)];
        var validNumbers = preamble
            .SelectMany(number => preamble.Select(otherNumber => number + otherNumber))
            .ToHashSet();
        return !validNumbers.Contains(number);
    })
    .First();
Console.WriteLine($"The first invalid number is {invalidNumber}");

var weaknessRange = CalculateWeaknessRange(encryptedNumbers, invalidNumber);
Console.WriteLine($"The min and max sum of the weakness range is {weaknessRange.Min() + weaknessRange.Max()}");

ICollection<long> CalculateWeaknessRange(ICollection<long> encryptedNumbers, long invalidNumber)
{
    for (int index = 0; index < encryptedNumbers.Count; index++)
    {
        long sum = 0;
        var weaknessRange = encryptedNumbers
            .Skip(index)
            .TakeWhile(number => (sum += number) <= invalidNumber)
            .ToArray();

        if (sum == invalidNumber)
            return weaknessRange;
    };
    return null;
}

static IEnumerable<long> ReadInNumbers()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while ((line = fileReader.ReadLine()) != null)
        yield return long.Parse(line);
}
