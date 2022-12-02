
var strategies = InputStrategy().ToList();

var totalScore = strategies
    .Select(strategy => WinScore(strategy.opponent, strategy.self) + (int)strategy.self + 1)
    .Sum();

Console.WriteLine($"The totalScore is {totalScore}");

var orders = InputOrders().ToList();

var orderedScore = orders
    .Select(order => (opponent: order.opponent, self: SelectChoice(order.opponent, order.self)))
    .Select(strategy => WinScore(strategy.opponent, strategy.self) + (int)strategy.self + 1)
    .Sum();

Console.WriteLine($"The orderedScore is {orderedScore}");

static int WinScore(Choice opponent, Choice self)
{
    return opponent switch
    {
        Choice.Rock => self switch
        {
            Choice.Rock => 3,
            Choice.Paper => 6,
            Choice.Scissors => 0
        },

        Choice.Paper => self switch
        {
            Choice.Rock => 0,
            Choice.Paper => 3,
            Choice.Scissors => 6
        },

        Choice.Scissors => self switch
        {
            Choice.Rock => 6,
            Choice.Paper => 0,
            Choice.Scissors => 3
        },
    };
}

static Choice SelectChoice(Choice opponent, Result self)
{
    return opponent switch
    {
        Choice.Rock => self switch
        {
            Result.Lose => Choice.Scissors,
            Result.Draw => Choice.Rock,
            Result.Win => Choice.Paper
        },

        Choice.Paper => self switch
        {
            Result.Lose => Choice.Rock,
            Result.Draw => Choice.Paper,
            Result.Win => Choice.Scissors
        },

        Choice.Scissors => self switch
        {
            Result.Lose => Choice.Paper,
            Result.Draw => Choice.Scissors,
            Result.Win => Choice.Rock
        },
    };
}

static IEnumerable<(Choice opponent, Choice self)> InputStrategy()
{
    using var fileReader = new StreamReader("input.txt");
    string? line;

    while ((line = fileReader.ReadLine()) != null)
    {
        yield return (opponent: (Choice)(line[0] - 'A'), self: (Choice)(line[^1] - 'X'));
    }
}

static IEnumerable<(Choice opponent, Result self)> InputOrders()
{
    using var fileReader = new StreamReader("input.txt");
    string? line;

    while ((line = fileReader.ReadLine()) != null)
    {
        yield return (opponent: (Choice)(line[0] - 'A'), self: (Result)(line[^1] - 'X'));
    }
}


public enum Choice
{
    Rock = 0,
    Paper = 1,
    Scissors = 2
}

public enum Result
{
    Lose = 0,
    Draw = 1,
    Win = 2
}