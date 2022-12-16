var monkeys = CreateMonkeys();

for (var i = 0; i < 20; i++)
    foreach (var monkey in monkeys)
        monkey.TakeTurn();

var monkeyBusiness = monkeys
    .Select(m => m.InspectionCount)
    .OrderDescending()
    .Take(2)
    .Aggregate((x, y) => x * y);
Console.WriteLine($"The monkey business is {monkeyBusiness}");

monkeys = CreateMonkeys();
var divisor = monkeys
    .Select(monkey => monkey.TestNumber)
    .Aggregate((x, y) => x * y);

for (var i = 0; i < 10000; i++)
    foreach (var monkey in monkeys)
        monkey.TakeWorriedTurn(divisor);

var worriedMonkeyBusiness = monkeys
    .Select(m => m.InspectionCount)
    .OrderDescending()
    .Take(2)
    .Aggregate((x, y) => x * y);
Console.WriteLine($"The worried monkey business is {worriedMonkeyBusiness}");

static Monkey[] CreateTestMonkeys()
{
    var monkey0 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 79, 98 }),
        Operation = old => old * 19,
        TestNumber = 23
    };
    var monkey1 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 54, 65, 75, 74 }),
        Operation = old => old + 6,
        TestNumber = 19
    };
    var monkey2 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 79, 60, 97 }),
        Operation = old => old * old,
        TestNumber = 13
    };
    var monkey3 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 74 }),
        Operation = old => old + 3,
        TestNumber = 17
    };
    monkey0.Success = monkey2;
    monkey0.Fail = monkey3;
    monkey1.Success = monkey2;
    monkey1.Fail = monkey0;
    monkey2.Success = monkey1;
    monkey2.Fail = monkey3;
    monkey3.Success = monkey0;
    monkey3.Fail = monkey1;

    return new[] { monkey0, monkey1, monkey2, monkey3 };
}

static Monkey[] CreateMonkeys()
{
    var monkey0 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 63, 84, 80, 83, 84, 53, 88, 72 }),
        Operation = old => old * 11,
        TestNumber = 13
    };
    var monkey1 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 67, 56, 92, 88, 84 }),
        Operation = old => old + 4,
        TestNumber = 11
    };
    var monkey2 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 52 }),
        Operation = old => old * old,
        TestNumber = 2
    };
    var monkey3 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 59, 53, 60, 92, 69, 72 }),
        Operation = old => old + 2,
        TestNumber = 5
    };
    var monkey4 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 61, 52, 55, 61 }),
        Operation = old => old + 3,
        TestNumber = 7
    };
    var monkey5 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 79, 53 }),
        Operation = old => old + 1,
        TestNumber = 3
    };
    var monkey6 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 59, 86, 67, 95, 92, 77, 91 }),
        Operation = old => old + 5,
        TestNumber = 19
    };
    var monkey7 = new Monkey
    {
        Items = new Queue<ulong>(new ulong[] { 58, 83, 89 }),
        Operation = old => old * 19,
        TestNumber = 17
    };

    monkey0.Success = monkey4;
    monkey0.Fail = monkey7;
    monkey1.Success = monkey5;
    monkey1.Fail = monkey3;
    monkey2.Success = monkey3;
    monkey2.Fail = monkey1;
    monkey3.Success = monkey5;
    monkey3.Fail = monkey6;
    monkey4.Success = monkey7;
    monkey4.Fail = monkey2;
    monkey5.Success = monkey0;
    monkey5.Fail = monkey6;
    monkey6.Success = monkey4;
    monkey6.Fail = monkey0;
    monkey7.Success = monkey2;
    monkey7.Fail = monkey1;

    return new[] { monkey0, monkey1, monkey2, monkey3, monkey4, monkey5, monkey6, monkey7 };
}

public class Monkey
{
    public required Queue<ulong> Items { get; init; }
    public required Func<ulong, ulong> Operation { get; init; }
    public required ulong TestNumber { get; init; }
    public Monkey Success { get; set; } = null!;
    public Monkey Fail { get; set; } = null!;

    public ulong InspectionCount { get; private set; } = 0;

    public void TakeTurn()
    {
        while (Items.TryDequeue(out var item))
            checked
            {
                var currentScore = Operation(item) / 3;
                InspectionCount++;
                if (currentScore % TestNumber == 0)
                    Success.Items.Enqueue(currentScore);
                else
                    Fail.Items.Enqueue(currentScore);
            }
    }

    public void TakeWorriedTurn(ulong divisor)
    {
        while (Items.TryDequeue(out var item))
        {
            var currentScore = Operation(item) % divisor;
            InspectionCount++;
            if (currentScore % TestNumber == 0)
                Success.Items.Enqueue(currentScore);
            else
                Fail.Items.Enqueue(currentScore);
        }
    }
}