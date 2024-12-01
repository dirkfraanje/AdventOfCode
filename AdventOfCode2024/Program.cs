using System.Diagnostics;

var input = File.ReadAllLines("inputDay1.txt");
var list1 = new List<int>();
var list2 = new List<int>();
foreach (var line in input)
{
    var values = line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();

    list1.Add(values[0]);
    list2.Add(values[1]);
}

// Part 1
list1.Sort();
list2.Sort();

var resultPart1 = list1.Zip(list2, (x, y) => Math.Abs(x - y)).Sum();
Console.WriteLine($"Result part 1: {resultPart1}");

// Part 2
var resultPart2 = 0;
var elementCounts = list2.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
foreach (var item in list1)
{
    if (elementCounts.TryGetValue(item, out var count))
    {
        resultPart2 += item * count;
    }
}

Console.WriteLine($"Result part 2: {resultPart2}");
Console.ReadLine();