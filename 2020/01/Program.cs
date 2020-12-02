using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

var numbers = InputNumbers().OrderBy(item => item).ToList();
FindTwoNumbers(numbers);

foreach (var firstNumber in numbers)
{
    foreach (var firstSum in numbers.Select(number => number + firstNumber).Where(sum => sum < 2020))
    {
        var thirdNumber = 2020 - firstSum;
        if (numbers.Contains(thirdNumber))
        {
            var secoundNumber = firstSum - firstNumber;
            Console.Out.WriteLine($"The numbers are {firstNumber}, {secoundNumber} and {thirdNumber}. The result is {firstNumber * secoundNumber * thirdNumber}");
            return;
        }
    }
}

static void FindTwoNumbers(List<int> numbers)
{
    foreach (var number in numbers)
    {
        var otherNumber = 2020 - number;
        if (numbers.Contains(otherNumber))
        {
            Console.Out.WriteLine($"The numbers are {number} and {otherNumber}. The result is {number * otherNumber}");
            break;
        }
    }
}

static IEnumerable<int> InputNumbers()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    while((line = fileReader.ReadLine()) != null){
        yield return int.Parse(line);
    }
}
