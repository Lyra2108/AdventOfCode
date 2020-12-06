using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

Console.WriteLine($"There are {ReadInJoinedAnswers().Select(set => set.Count).Sum()} yes-answers.");
Console.WriteLine($"There are {ReadInAnswersAllYes().Select(set => set.Count).Sum()} yes-answers from every-one.");


static IEnumerable<ISet<char>> ReadInJoinedAnswers()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    var answers = new HashSet<char>();
    while ((line = fileReader.ReadLine()) != null)
    {
        if (line == string.Empty)
        {
            var temp = answers;
            answers = new HashSet<char>();
            yield return temp;
        }
        else
        {
            answers = answers.Concat(line.ToCharArray()).ToHashSet();
        }

    }
    yield return answers;
}

static IEnumerable<ISet<char>> ReadInAnswersAllYes()
{
    using var fileReader = new StreamReader("input.txt");
    string line;
    HashSet<char> answers = null;
    while ((line = fileReader.ReadLine()) != null)
    {
        if (line == string.Empty)
        {
            var temp = answers;
            answers = null;
            yield return temp;
        }
        else
        {
            if (answers == null)
            {
                answers = line.ToCharArray().ToHashSet();
            }
            else
            {
                answers = answers.Intersect(line.ToCharArray()).ToHashSet();
            }
        }
    }
    yield return answers;
}