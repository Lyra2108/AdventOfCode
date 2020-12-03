using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

var lines = ReadInLines();

var trees = GetTrees(lines, 3, 1);
Console.WriteLine($"It will encounter {trees}");

var treesForConfigurations = new List<(int left, int down)>{
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2),
    }
    .Select(tuple => GetTrees(lines, tuple.left, tuple.down))
    .Aggregate((result, item) => result * item);
Console.WriteLine($"It will encounter {treesForConfigurations}");

long GetTrees(IEnumerable<string> lines, int left, int down)
{
    return lines
        .Where((lines, i) => i % down == 0)
        .Select((line, i) =>
        {
            var mapLine = line.ToCharArray();
            return mapLine[(i * left) % mapLine.Length] == '#' ? 1 : 0;
        })
        .Sum();
}

static IEnumerable<string> ReadInLines()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while ((line = fileReader.ReadLine()) != null)
    {
        yield return line;
    }
}
