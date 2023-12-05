using System.Text.RegularExpressions;

var sum = PartNumbers().Sum();

Console.WriteLine($"The sum is {sum}");

var gears = GearRatios().Sum();

Console.WriteLine($"The sum is {gears}");

IEnumerable<int> PartNumbers()
{
    var numberRegex = new Regex(@"\d+");
    using var fileReader = new StreamReader("input.txt");
    var lines = fileReader.ReadToEnd()
        .Split('\n')
        .Select(line => line.Trim())
        .ToArray();
    var machine = lines
        .Select(line => line.ToCharArray())
        .ToArray();

    foreach (var (line, lineNumber) in lines.Select((line, i) => (line, i)))
    {
        var matches = numberRegex.Matches(line);
        foreach (Match match in matches)
        {
            if (IsPart())
                yield return int.Parse(match.Value);

            bool IsPart()
            {
                var startX = Math.Max(match.Index - 1, 0);
                var endX = Math.Min(match.Index + match.Length, machine[0].Length - 1);
                var startY = Math.Max(lineNumber - 1, 0);
                var endY = Math.Min(lineNumber + 1, machine.Length - 1);
                for (var y = startY; y <= endY; y++)
                for (var x = startX; x <= endX; x++)
                    if (machine[y][x] is not '.' and not (>= '0' and <= '9'))
                        return true;

                return false;
            }
        }
    }
}

IEnumerable<int> GearRatios()
{
    var gearRegex = new Regex(@"\*");
    using var fileReader = new StreamReader("input.txt");
    var lines = fileReader.ReadToEnd()
        .Split('\n')
        .Select(line => line.Trim())
        .ToArray();
    var machine = lines
        .Select(line => line.ToCharArray())
        .ToArray();

    foreach (var (line, lineNumber) in lines.Select((line, i) => (line, i)))
    {
        foreach (Match gear in gearRegex.Matches(line))
        {
            if (IsGear(out var first, out var second))
                yield return first * second;

            bool IsGear(out int first, out int second)
            {
                var startX = Math.Max(gear.Index - 1, 0);
                var endX = Math.Min(gear.Index + 1, machine[0].Length - 1);
                var startY = Math.Max(lineNumber - 1, 0);
                var endY = Math.Min(lineNumber + 1, machine.Length - 1);

                int? firstNumber = null;
                for (var y = startY; y <= endY; y++)
                for (var x = startX; x <= endX; x++)
                    if (machine[y][x] is >= '0' and <= '9')
                    {
                        if (firstNumber is null)
                        {
                            FindNumber(out firstNumber);
                        }
                        else
                        {
                            FindNumber(out var secondNumber);

                            if (secondNumber != firstNumber)
                            {
                                first = firstNumber.Value;
                                second = secondNumber!.Value;

                                return true;
                            }
                        }

                        void FindNumber(out int? number)
                        {
                            var numberString = "" + machine[y][x];
                            var search = x - 1;
                            while (search >= 0 && machine[y][search] is >= '0' and <= '9')
                                numberString = machine[y][search--] + numberString;
                            search = x + 1;
                            while (search < machine[0].Length && machine[y][search] is >= '0' and <= '9')
                                numberString += machine[y][search++];
                            number = int.Parse(numberString);
                        }
                    }


                first = 0;
                second = 0;
                return false;
            }
        }
    }
}