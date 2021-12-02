using System.Text.RegularExpressions;


var directions = InputNumbers().ToList();
var currentPosition = (horizontal: 0, dept: 0);
foreach ((string direction, int unit) in directions)
{
  switch (direction)
  {
    case "down":
      currentPosition.dept += unit;
      break;
    case "up":
      currentPosition.dept -= unit;
      break;
    case "forward":
      currentPosition.horizontal += unit;
      break;
  }
}

Console.WriteLine(currentPosition.horizontal * currentPosition.dept);

var currentPositionWithAim = (horizontal: 0, dept: 0, aim: 0);
foreach ((string direction, int unit) in directions)
{
  switch (direction)
  {
    case "down":
      currentPositionWithAim.aim += unit;
      break;
    case "up":
      currentPositionWithAim.aim -= unit;
      break;
    case "forward":
      currentPositionWithAim.horizontal += unit;
      currentPositionWithAim.dept += currentPositionWithAim.aim * unit;
      break;
  }
}

Console.WriteLine(currentPositionWithAim.horizontal * currentPositionWithAim.dept);

static IEnumerable<(string direction, int unit)> InputNumbers()
{
  var regex = new Regex(@"^(?'direction'\w+) (?'units'\d+)$");
  using var fileReader = new StreamReader("input.txt");
  string? line;
  while ((line = fileReader.ReadLine()) != null)
  {
    var match = regex.Match(line);
    if (match.Success)
      yield return (match.Groups["direction"].Value, int.Parse(match.Groups["units"].Value));
  }
}