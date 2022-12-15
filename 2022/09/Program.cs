#pragma warning disable CS8509

var steps = Steps().ToArray();
var currentHead = (x: 0, y: 0);
var currentTail = (x: 0, y: 0);

var places = MoveKnots(currentHead, currentTail);
Console.WriteLine($"The tail visited {places} places");

var moreKnots = MoveKnots(
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0),
    (x: 0, y: 0)
);
Console.WriteLine($"The 10. tail visited {moreKnots} places");


int MoveKnots(params (int x, int y)[] knots)
{
    var visited = new HashSet<(int x, int y)> { knots[^1] };

    foreach (var (direction, stepsToTake) in steps)
        for (var i = 0; i < stepsToTake; i++)
        {
            knots[0] = direction switch
            {
                'U' => (knots[0].x, knots[0].y + 1),
                'D' => (knots[0].x, knots[0].y - 1),
                'L' => (knots[0].x - 1, knots[0].y),
                'R' => (knots[0].x + 1, knots[0].y),
            };

            for (var followI = 1; followI < knots.Length; followI++)
            {
                var parent = knots[followI - 1];
                var followingKnot = knots[followI];
                if (MoveIfNeeded(parent, ref followingKnot))
                {
                    knots[followI] = followingKnot;
                    if (followI == knots.Length - 1)
                        visited.Add(followingKnot);
                }
            }
        }

    return visited.Count;
}

bool MoveIfNeeded((int x, int y) head, ref (int x, int y) tail)
{
    if (head == tail)
        return false;

    var xDistance = head.x - tail.x;
    var yDistance = head.y - tail.y;
    if (int.Abs(xDistance) <= 1 && int.Abs(yDistance) <= 1)
        return false;

    var moveX = int.Abs(xDistance) <= 1 ? xDistance : xDistance - int.Sign(xDistance);
    var moveY = int.Abs(yDistance) <= 1 ? yDistance : yDistance - int.Sign(yDistance);
    tail = (tail.x + moveX, tail.y + moveY);
    return true;
}

IEnumerable<(char direction, int steps)> Steps()
{
    using var reader = new StreamReader("input.txt");
    while (reader.ReadLine() is { } line)
        yield return (direction: line[0], steps: int.Parse(line[2..]));
}