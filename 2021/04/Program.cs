var lines = File.ReadLines("input.txt");

var numbers = lines.First().Split(',').Select(numberString => int.Parse(numberString)).ToArray();

lines = lines.Skip(1);
var boards = Enumerable.Range(0, lines.Count(string.IsNullOrEmpty)).Select((line, i) => new Board(lines.Skip(i * 6 + 1).Take(5))).ToArray();

Task1();

boards = Enumerable.Range(0, lines.Count(string.IsNullOrEmpty)).Select((line, i) => new Board(lines.Skip(i * 6 + 1).Take(5))).ToArray();
Task2();

void Task1()
{

    foreach (var number in numbers)
    {
        foreach (var board in boards)
        {
            var hasHit = board.ProcessNumber(number);

            if (hasHit)
            {
                Console.WriteLine(board.Sum(number));
                return;
            }
        }
    }
}



void Task2()
{
    var boardsLeft = boards.Length;
    foreach (var number in numbers)
    {
        foreach (var board in boards.Where(board => !board.HasBeenHit))
        {
            var hasHit = board.ProcessNumber(number);

            if (hasHit)
            {
                boardsLeft--;
                if(boardsLeft == 0)
                {
                    Console.WriteLine(board.Sum(number));
                    return;
                }
            }
        }
    }
}

class Board
{
    public BingoNumber[][] Numbers { get; }

    public bool HasBeenHit { get; set; }

    public Board(IEnumerable<string> lines)
    {
        Numbers = lines
        .Select(numberString => numberString
            .Split(' ')
            .Where(line => !string.IsNullOrEmpty(line))
            .Select(number =>
            {
                return new BingoNumber { Number = int.Parse(number) };
            }).ToArray()
        ).ToArray();
    }

    public bool ProcessNumber(int number)
    {
        for (int x = 0; x < Numbers.Length; x++)
        {
            for (int y = 0; y < Numbers[x].Length; y++)
            {
                var bingoNumber = Numbers[x][y];
                if (bingoNumber.Number != number)
                    continue;

                bingoNumber.Hit = true;

                var boardHit = Numbers[x].All(bingo => bingo.Hit)
                    || Numbers.Select(row => row[y]).All(bingo => bingo.Hit);

                if(boardHit)
                    HasBeenHit = true;

                return boardHit;
            }
        }
        return false;
    }

    public int Sum(int number)
    {
        return Numbers.SelectMany(row => row).Where(bingo => !bingo.Hit).Sum(bingo => bingo.Number) * number;
    }
}

class BingoNumber
{
    public int Number { get; init; }
    public bool Hit { get; set; }
}
