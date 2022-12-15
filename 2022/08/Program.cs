var trees = Trees();
var xDimension = trees.Length;
var yDimension = trees[0].Length;


for (var i = 0; i < trees.Length; i++)
for (var j = 0; j < trees[i].Length; j++)
    CalculateVisibility(i, j);

var sum = trees.Select(treeSum => treeSum.Count(tree => tree.Visible!.Value)).Sum();
Console.WriteLine($"There are {sum} visible trees.");

for (var i = 0; i < trees.Length; i++)
for (var j = 0; j < trees[i].Length; j++)
    CalculateViewingDistance(i, j);

var maxAreaScore = trees.Select(t => t.Max(tree => tree.ViewingDistance)).Max();
Console.WriteLine($"The highest area score is {maxAreaScore}.");

void CalculateVisibility(int x, int y)
{
    var tree = trees[x][y];
    if (x == 0 || x == xDimension - 1 || y == 0 || y == yDimension - 1)
    {
        tree.Visible = true;
        return;
    }

    if (tree.Height == 0)
    {
        tree.Visible = false;
        return;
    }

    var top = trees[x][..y].All(t => t.Height < tree.Height);
    if (top)
    {
        tree.Visible = true;
        return;
    }

    var bottom = trees[x][(y + 1)..].All(t => t.Height < tree.Height);
    if (bottom)
    {
        tree.Visible = true;
        return;
    }

    var left = trees[..x].Select(t => t[y]).All(t => t.Height < tree.Height);
    if (left)
    {
        tree.Visible = true;
        return;
    }

    var right = trees[(x + 1)..].Select(t => t[y]).All(t => t.Height < tree.Height);
    tree.Visible = right;
}

void CalculateViewingDistance(int x, int y)
{
    var tree = trees[x][y];

    var top = CountViewingDistance(trees[x][..y].Reverse(), tree);
    var bottom = CountViewingDistance(trees[x][(y + 1)..], tree);
    var left = CountViewingDistance(trees[..x].Select(t => t[y]).Reverse(), tree);
    var right = CountViewingDistance(trees[(x + 1)..].Select(t => t[y]), tree);

    tree.ViewingDistance = top * bottom * left * right;
}

int CountViewingDistance(IEnumerable<Tree> treesInDirection, Tree tree)
{
    var higherTree = false;
    var countViewingDistance = treesInDirection
        .TakeWhile(t =>
        {
            if (t.Height < tree.Height)
                return true;
            if (t.Height >= tree.Height) higherTree = true;

            return false;
        }).Count();
    if (higherTree)
        countViewingDistance++;
    return countViewingDistance;
}

static Tree[][] Trees()
{
    using var reader = new StreamReader("input.txt");
    return reader.ReadToEnd()
        .Split("\n")
        .Select(line => line.ToCharArray()
            .Select(c => new Tree
            {
                Height = int.Parse(c.ToString())
            })
            .ToArray())
        .ToArray();
}

public class Tree
{
    public required int Height { get; init; }
    public bool? Visible { get; set; }
    public int? ViewingDistance { get; set; }
}