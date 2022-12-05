// [Q] [J]                         [H]
// [G] [S] [Q]     [Z]             [P]
// [P] [F] [M]     [F]     [F]     [S]
// [R] [R] [P] [F] [V]     [D]     [L]
// [L] [W] [W] [D] [W] [S] [V]     [G]
// [C] [H] [H] [T] [D] [L] [M] [B] [B]
// [T] [Q] [B] [S] [L] [C] [B] [J] [N]
// [F] [N] [F] [V] [Q] [Z] [Z] [T] [Q]
//  1   2   3   4   5   6   7   8   9

using System.Text.RegularExpressions;

var inputStacks = new[]
{
    new Stack<char>(new[] { 'Q', 'G', 'P', 'R', 'L', 'C', 'T', 'F' }.Reverse()),
    new Stack<char>(new[] { 'J', 'S', 'F', 'R', 'W', 'H', 'Q', 'N' }.Reverse()),
    new Stack<char>(new[] { 'Q', 'M', 'P', 'W', 'H', 'B', 'F' }.Reverse()),
    new Stack<char>(new[] { 'F', 'D', 'T', 'S', 'V' }.Reverse()),
    new Stack<char>(new[] { 'Z', 'F', 'V', 'W', 'D', 'L', 'Q' }.Reverse()),
    new Stack<char>(new[] { 'S', 'L', 'C', 'Z' }.Reverse()),
    new Stack<char>(new[] { 'F', 'D', 'V', 'M', 'B', 'Z' }.Reverse()),
    new Stack<char>(new[] { 'B', 'J', 'T' }.Reverse()),
    new Stack<char>(new[] { 'H', 'P', 'S', 'L', 'G', 'B', 'N', 'Q' }.Reverse()),
};

foreach (var (from, to, times) in Instructions())
    for (var i = 0; i < times; i++)
        inputStacks[to].Push(inputStacks[from].Pop());

var topStacks = string.Concat(inputStacks.Select(stack => stack.Pop()));

Console.WriteLine($"The top stacks are {topStacks}");

var inputStacks2 = new[]
{
    new Stack<char>(new[] { 'Q', 'G', 'P', 'R', 'L', 'C', 'T', 'F' }.Reverse()),
    new Stack<char>(new[] { 'J', 'S', 'F', 'R', 'W', 'H', 'Q', 'N' }.Reverse()),
    new Stack<char>(new[] { 'Q', 'M', 'P', 'W', 'H', 'B', 'F' }.Reverse()),
    new Stack<char>(new[] { 'F', 'D', 'T', 'S', 'V' }.Reverse()),
    new Stack<char>(new[] { 'Z', 'F', 'V', 'W', 'D', 'L', 'Q' }.Reverse()),
    new Stack<char>(new[] { 'S', 'L', 'C', 'Z' }.Reverse()),
    new Stack<char>(new[] { 'F', 'D', 'V', 'M', 'B', 'Z' }.Reverse()),
    new Stack<char>(new[] { 'B', 'J', 'T' }.Reverse()),
    new Stack<char>(new[] { 'H', 'P', 'S', 'L', 'G', 'B', 'N', 'Q' }.Reverse()),
};


foreach (var (from, to, times) in Instructions())
{
    var mover = new Stack<char>();
    for (var i = 0; i < times; i++) 
        mover.Push(inputStacks2[from].Pop());

    for (var i = 0; i < times; i++) 
        inputStacks2[to].Push(mover.Pop());
}

var topStack2 = string.Concat(inputStacks2.Select(stack => stack.Pop()));

Console.WriteLine($"The top stacks with the mover are {topStack2}");

static IEnumerable<(int from, int to, int times)> Instructions()
{
    var regex = new Regex(@"move (?'times'\d+) from (?'from'\d+) to (?'to'\d+)");
    using var fileReader = new StreamReader("input.txt");

    while (fileReader.ReadLine() is { } line)
    {
        var match = regex.Match(line);
        yield return (
            from: int.Parse(match.Groups["from"].Value) - 1,
            to: int.Parse(match.Groups["to"].Value) - 1,
            times: int.Parse(match.Groups["times"].Value)
        );
    }
}