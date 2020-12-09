using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

var operations = ReadInLines().Select(line => new Operation(line)).ToArray();
var executedOperations = ExecuteOperations(operations.Select(x => x.Clone()).ToArray(), out var accumulator, out bool _).ToArray();

Console.WriteLine($"The result without fix is: {accumulator}");

var fixedAccumulator = 0;
var _ = executedOperations.First(executed =>
{
    if (executed.operation.Code == "acc")
        return false;

    var fixedOperations = operations.Select(x => x.Clone()).ToArray();
    fixedOperations[executed.index].Code = executed.operation.Code == "nop" ? "jmp" : "nop";

    ExecuteOperations(fixedOperations, out fixedAccumulator, out bool looped);
    return !looped;
});
Console.WriteLine($"The result with fix is: {fixedAccumulator}");

static ICollection<(Operation operation, int index)> ExecuteOperations(Operation[] operations, out int accumulator, out bool looped)
{
    accumulator = 0;
    looped = false;
    int pointer = 0;
    var executedOperations = new List<(Operation operation, int index)>();
    for (var operation = operations[pointer]; operation != null; operation = operations[pointer])
    {
        operations[pointer] = null;
        var oldPointer = pointer;

        operation.Execute(ref accumulator, ref pointer);
        executedOperations.Add((operation, oldPointer));

        if (pointer == oldPointer)
            pointer++;

        if (pointer >= operations.Length)
            return executedOperations;
    }
    looped = true;
    return executedOperations;
}

static IEnumerable<string> ReadInLines()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while ((line = fileReader.ReadLine()) != null)
    {
        if (!string.IsNullOrWhiteSpace(line))
            yield return line;
    }
}

class Operation
{
    private int Value { get; init; }

    public string Code { get; set; }

    private Operation()
    {
    }
    public Operation(string line)
    {
        Code = line[0..3];
        Value = int.Parse(line[4..^0]);
    }

    public Operation Clone()
    {
        return new Operation { Code = Code, Value = Value };
    }

    public void Execute(ref int accumulator, ref int pointer)
    {
        switch (Code)
        {
            case "acc":
                accumulator += Value;
                break;
            case "jmp":
                pointer += Value;
                break;
        };
    }
}
