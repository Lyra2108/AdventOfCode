var sonarValues = InputNumbers()
  .ToArray();

var increases = sonarValues
  .Zip(sonarValues.Skip(1), (first, second) => second > first)
  .Where(increase => increase)
  .Count();

Console.WriteLine($"The number increases {increases} times");

var threeMeasurementWindow = sonarValues
  .Zip(sonarValues.Skip(1), sonarValues.Skip(2))
  .Select(tuple => tuple.Item1 + tuple.Item2 + tuple.Item3)
  .ToArray();

var largerSums = threeMeasurementWindow
  .Zip(threeMeasurementWindow.Skip(1), (first, second) => second > first)
  .Where(increase => increase)
  .Count();

Console.WriteLine($"The sum increases {largerSums} times");

static IEnumerable<int> InputNumbers()
{
  using var fileReader = new StreamReader("input.txt");
  string line;
  while ((line = fileReader.ReadLine()) != null)
  {
    yield return int.Parse(line);
  }
}
