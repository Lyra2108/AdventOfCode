using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

var seats = ReadInSeats().ToArray();

var changed = false;
char[][] result = seats;
do
{
    changed = false;
    result = result
        .Select((row, rowIndex) => row
            .Select((seat, columnIndex) => TransitionSeat(seat, rowIndex, columnIndex, ref changed))
            .ToArray()
        )
        .ToArray();
} while (changed);

var occupied = result.SelectMany(row => row).Count(seat => seat == '#');
Console.WriteLine(occupied);

char TransitionSeat(char seat, int rowIndex, int columnIndex, ref bool changed)
{
    if (seat == '.'){
        return seat;
    }

    var lowerRow = Math.Clamp(rowIndex - 1, 0, seats.Count());
    var upperRow = Math.Clamp(rowIndex + 2, 0, seats.Count());
    var lowerColumn = Math.Clamp(columnIndex - 1, 0, seats[rowIndex].Count());
    var upperColumn = Math.Clamp(columnIndex + 2, 0, seats[rowIndex].Count());

    var occupied = result[lowerRow..upperRow]
        .SelectMany(row => row[lowerColumn..upperColumn])
        .Count(seat => seat == '#');
    occupied -= seat == '#' ? 1 : 0;

    if (seat == 'L' && occupied == 0)
    {
        changed = true;
        return '#';
    }
    if (seat == '#' && occupied >= 4)
    {
        changed = true;
        return 'L';
    }
    return seat;
}

char TransitionSeat2ndRule(char seat, int rowIndex, int columnIndext, ref bool changed)
{
    if (seat == '.'){
        return seat;
    }

    var lowerRow = Math.Clamp(rowIndex - 1, 0, seats.Count());
    var upperRow = Math.Clamp(rowIndex + 2, 0, seats.Count());
    var lowerColumn = Math.Clamp(columnIndex - 1, 0, seats[rowIndex].Count());
    var upperColumn = Math.Clamp(columnIndex + 2, 0, seats[rowIndex].Count());

    var occupied = result[lowerRow..upperRow]
        .SelectMany(row => row[lowerColumn..upperColumn])
        .Count(seat => seat == '#');
    occupied -= seat == '#' ? 1 : 0;

    if (seat == 'L' && occupied == 0)
    {
        changed = true;
        return '#';
    }
    if (seat == '#' && occupied >= 5)
    {
        changed = true;
        return 'L';
    }
    return seat;
}
static IEnumerable<char[]> ReadInSeats()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while ((line = fileReader.ReadLine()) != null)
        yield return line.ToCharArray();
}
