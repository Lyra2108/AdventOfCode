var elves = InputElves();

var max = elves
    .Select(elf => elf.Sum())
    .Max();

Console.WriteLine($"The max calories are {max}");

var topThree = elves
    .Select(elf => elf.Sum())
    .OrderDescending()
    .ToArray()[..3]
    .Sum();

Console.WriteLine($"The top three calories are {topThree}");

static IEnumerable<ICollection<int>> InputElves()
{
    using var fileReader = new StreamReader("input.txt");
    string? line;

    var calories = new List<int>();
    while ((line = fileReader.ReadLine()) != null)
    {
        if (line == string.Empty)
        {
            yield return calories;
            calories = new List<int>();
        }
        else
            calories.Add(int.Parse(line));
    }
}
