var start = (x: 20, y: 0);
var end = (x: 20, y: 43);

var map = Map();

map[start.x][start.y] = 'a';
map[end.x][end.y] = 'z';

var startSteps = new HashSet<(int steps, int x, int y)> { (0, start.x, start.y) };
var way = FindWay(startSteps);
Console.WriteLine($"There are {way} steps needed.");

var aLevel = FindAllAs(map).Select(point => (steps: 0, point.x, point.y)).ToHashSet();
var aWay = FindWay(aLevel);
Console.WriteLine($"There are {aWay} steps needed from any a level.");

int FindWay(HashSet<(int steps, int x, int y)> possibleSteps)
{
    var xDimension = map.Length;
    var yDimension = map[0].Length;

    var checkedSteps = new HashSet<(int x, int y)>();
    while (possibleSteps.Any())
    {
        var nextStep = possibleSteps.MinBy(s => s.steps);
        if ((nextStep.x, nextStep.y) == end)
            return nextStep.steps;

        if (nextStep.x > 1)
            CheckIfPossibleStep(nextStep, (nextStep.x - 1, nextStep.y));
        if (nextStep.x < xDimension - 1)
            CheckIfPossibleStep(nextStep, (nextStep.x + 1, nextStep.y));
        if (nextStep.y > 1)
            CheckIfPossibleStep(nextStep, (nextStep.x, nextStep.y - 1));
        if (nextStep.y < yDimension - 1)
            CheckIfPossibleStep(nextStep, (nextStep.x, nextStep.y + 1));

        checkedSteps.Add((nextStep.x, nextStep.y));
        possibleSteps.Remove(nextStep);
    }

    throw new Exception("No way found");

    void CheckIfPossibleStep((int steps, int x, int y) currentStep, (int x, int y) possibleNextStep)
    {
        if (checkedSteps.Contains(possibleNextStep))
            return;

        var currentHeight = map[currentStep.x][currentStep.y];
        var height = map[possibleNextStep.x][possibleNextStep.y];
        if (height <= currentHeight + 1)
            possibleSteps.Add((currentStep.steps + 1, possibleNextStep.x, possibleNextStep.y));
    }
}

IEnumerable<(int x, int y)> FindAllAs(char[][] maps)
{
    for (var x = 0; x < maps.Length; x++)
    for (var y = 0; y < maps[x].Length; y++)
        if (map[x][y] == 'a')
            yield return (x, y);
}


static char[][] Map()
{
    using var reader = new StreamReader("input.txt");
    return reader.ReadToEnd()
        .Split("\n")
        .Select(line => line.ToCharArray())
        .ToArray();
}