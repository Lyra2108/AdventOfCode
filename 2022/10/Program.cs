var display = new char[40, 6];

var strength = CalculateStrength(new HashSet<int> { 20, 60, 100, 140, 180, 220 }).Sum();
Console.WriteLine($"The strength is {strength}");

for (var j = 0; j < 6; j++)
{
    for (var i = 0; i < 40; i++)
        Console.Write(display[i, j]);
    Console.WriteLine();
}


IEnumerable<int> CalculateStrength(HashSet<int> needs)
{
    var cycle = 1;
    var x = 1;
    foreach (var (i, instruction) in Instructions().Select((s, i) => (i, s)))
    {
        if (needs.Contains(cycle))
            yield return cycle * x;

        if (instruction == "noop")
        {
            UpdateDisplay(cycle, x);
            cycle++;
            continue;
        }

        UpdateDisplay(cycle, x);
        cycle++;
        if (needs.Contains(cycle))
            yield return cycle * x;

        var add = int.Parse(instruction[5..]);
        UpdateDisplay(cycle, x);
        cycle++;
        x += add;
    }
}

void UpdateDisplay(int cycle, int x)
{
    var row = (cycle - 1) / 40;
    var column = (cycle - 1) % 40;
    var pixel = column >= x - 1 && column <= x + 1 ? '#' : '.';
    display[column, row] = pixel;
}

static IEnumerable<string> Instructions()
{
    using var reader = new StreamReader("input.txt");
    while (reader.ReadLine() is { } line)
        yield return line;
}