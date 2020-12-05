using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;


var example = new Boardingpass("FBFBBFFRLR");
Console.WriteLine(example.ToString());

var boardingPasses = ReadInLines()
    .Select(line => new BinaryBoardingpass(line))
    .ToList();

var maxSeat = boardingPasses.Max(pass => pass.SeatId);
var minSeat = boardingPasses.Min(pass => pass.SeatId);
Console.WriteLine($"The heighest Id is {maxSeat}");

var takenSeats = boardingPasses.Select(pass => pass.SeatId).ToArray();
var possibleSeat = Enumerable.Range(minSeat, maxSeat)
    .Except(takenSeats)
    .FirstOrDefault(seat => takenSeats.Contains(seat - 1) && takenSeats.Contains(seat + 1));
Console.WriteLine($"My seat is {possibleSeat}.");

static IEnumerable<string> ReadInLines()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while ((line = fileReader.ReadLine()) != null)
    {
        yield return line;
    }
}

class BinaryBoardingpass
{
    public int SeatId { get; }
    public BinaryBoardingpass(string line)
    {
        var binaryLine = string.Concat(line.ToCharArray()
            .Select(letter => letter switch
            {
                'R' => 1,
                'B' => 1,
                _ => 0
            }));

        SeatId = Convert.ToInt32(binaryLine, 2);
    }
}
class Boardingpass
{

    public int Row { get; }
    public int Column { get; }
    public int SeatId => Row * 8 + Column;

    public Boardingpass(string line)
    {
        var binaryLine = line.ToCharArray()
            .Select(letter => letter switch
            {
                'R' => true,
                'B' => true,
                _ => false
            }).ToArray();

        Row = CalculateNumber(binaryLine[0..7], 127);
        Column = CalculateNumber(binaryLine[7..^0], 7);
    }

    private int CalculateNumber(bool[] binaryLine, int max, int min = 0)
    {
        if (binaryLine.Length == 1)
            return binaryLine[0] ? max : min;

        var half = ((double)max + min) / 2;
        if (binaryLine[0])
            return CalculateNumber(binaryLine[1..^0], max, (int)Math.Ceiling(half));

        return CalculateNumber(binaryLine[1..^0], (int)Math.Floor(half), min);
    }

    public override string ToString()
    {
        return $"BoardingPass: Row: {Row}, Column: {Column}, Id: {SeatId}";
    }
}