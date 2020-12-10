using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

var adapters = ReadInAdapters()
    .OrderBy(x => x)
    .Prepend(0)
    .ToArray();

var differences = adapters
    .Zip(adapters.Skip(1), (first, secound) => secound - first)
    .ToLookup(x => x, x => x);
Console.WriteLine($"The number of 1-Jolt-Differences times 3-Jolt-Differences is: {differences[1].Count() * (differences[3].Count() + 1)}"); ;

var _permutationForAdapter = new Dictionary<int, long>();
Console.WriteLine($"There are {Permutations(adapters, 0)} possible permutations.");

long Permutations(int[] allAdapters, int currentAdapter)
{
    if (_permutationForAdapter.ContainsKey(currentAdapter))
        return _permutationForAdapter[currentAdapter];

    var index = Array.IndexOf(allAdapters, currentAdapter);
    var permutations = allAdapters
        .Skip(index + 1)
        .TakeWhile(adapter => adapter <= currentAdapter + 3)
        .Sum(nextAdapter => nextAdapter == allAdapters.Last() ? 1 : Permutations(allAdapters, nextAdapter));
    _permutationForAdapter[currentAdapter] = permutations;

    return permutations;
}

static IEnumerable<int> ReadInAdapters()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while ((line = fileReader.ReadLine()) != null)
        yield return int.Parse(line);
}
