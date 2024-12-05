using AdventOfCode2024;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

//Day1();
//Day2();
//Day3();
await Day4();

async Task Day4()
{
    var stopWatch = new Stopwatch();
    stopWatch.Start();
    var normal = "\x1b[39m";
    var green = "\x1b[92m";
    var input = File.ReadAllLines(@"Inputs\Day4.txt");
    var grid = new List<Coordinate>();
    for (int y = 0; y < input.Length; y++)
    {
        var line = input[y];
        var resultLine = new StringBuilder();

        for (int x = 0; x < line.Length; x++)
        {
            var letter = line[x];
            if (letter.Equals('X'))
            {
                grid.Add(new Coordinate(x, y, letter, green));
                resultLine.Append($"{green}{letter}{normal}");
            }

            else
            {
                grid.Add(new Coordinate(x, y, letter, normal));
                resultLine.Append($"{normal}{letter}{normal}");
            }

        }
        //Console.WriteLine(resultLine.ToString());
    }
    var result = 0;
    List<Task<int>> task = new List<Task<int>>();
    foreach (var item in grid.Where(c => c.Value.Equals('X')))
    {
        task.Add(CheckXMASResults(grid, item));
    }
    var resultFromTask = await Task.WhenAll(task);
    stopWatch.Stop();
    WriteResult(4, 1, $"Time:{stopWatch.ElapsedMilliseconds} - Result: {resultFromTask.Sum()}");
    
}
async Task<int> CheckXMASResults(List<Coordinate> grid, Coordinate item)
{
    var result = 0;
    // Go to top
    if (CheckForMas(grid, item.X, item.Y - 1, item.X, item.Y - 2, item.X, item.Y - 3))
        result++;

    // Go to diagonal top / right
    if (CheckForMas(grid, item.X + 1, item.Y - 1, item.X + 2, item.Y - 2, item.X + 3, item.Y - 3))
        result++;

    // Go right
    if (CheckForMas(grid, item.X + 1, item.Y, item.X + 2, item.Y, item.X + 3, item.Y))
        result++;

    // Go to diagonal right / bottom
    if (CheckForMas(grid, item.X + 1, item.Y + 1, item.X + 2, item.Y + 2, item.X + 3, item.Y + 3))
        result++;

    // Go bottom
    if (CheckForMas(grid, item.X, item.Y + 1, item.X, item.Y + 2, item.X, item.Y + 3))
        result++;

    // Go to diagonal bottom / left
    if (CheckForMas(grid, item.X - 1, item.Y + 1, item.X - 2, item.Y + 2, item.X - 3, item.Y + 3))
        result++;

    // Go left
    if (CheckForMas(grid, item.X - 1, item.Y, item.X - 2, item.Y, item.X - 3, item.Y))
        result++;

    // Go to diagonal left / top
    if (CheckForMas(grid, item.X - 1, item.Y - 1, item.X - 2, item.Y - 2, item.X - 3, item.Y - 3))
        result++;
    return result;
}

bool CheckForMas(List<Coordinate> grid, int x1, int y1, int x2, int y2, int x3, int y3)
{
    var valueM = grid.FirstOrDefault(c => c.X == x1 && c.Y == y1);
    if (valueM.Value != 'M')
        return false;

    var valueA = grid.FirstOrDefault(c => c.X == x2 && c.Y == y2);
    if (valueA.Value != 'A')
        return false;

    var valueS = grid.FirstOrDefault(c => c.X == x3 && c.Y == y3);
    if (valueS.Value != 'S')
        return false;
    return true;
}
void Day3()
{
    // Part 1
    var input = File.ReadAllText(@"Inputs\Day3.txt");
    var regexInstructions = new Regex("mul\\([0-9]+,[0-9]+\\)");
    var regexInstruction = new Regex("([0-9]+),([0-9]+)");
    var instructions = regexInstructions.Matches(input);
    var resultPart1 = 0;
    foreach (var instruction in instructions)
    {
        var instructionResult = regexInstruction.Matches(instruction.ToString());
        resultPart1 += int.Parse(instructionResult[0].Groups[1].Value) * int.Parse(instructionResult[0].Groups[2].Value);
    }

    WriteResult(3, 1, $"{resultPart1}");
    // Part 2
    var instructions2Pattern = @"mul\([0-9]+,[0-9]+\)|do\(\)|don't\(\)";
    var resultPart2 = 0;
    var instructionsEnabled = true;
    var matches = Regex.Matches(input, instructions2Pattern);
    foreach (var item in matches)
    {
        switch (item.ToString())
        {
            case "don't()":
                instructionsEnabled = false;
                break;
            case "do()":
                instructionsEnabled = true;
                break;
            default:
                if (!instructionsEnabled)
                    continue;
                var instructionResult = regexInstruction.Matches(item.ToString());
                resultPart2 += int.Parse(instructionResult[0].Groups[1].Value) * int.Parse(instructionResult[0].Groups[2].Value);
                break;
        }
    }
    WriteResult(3, 2, $"{resultPart2}");
}

static void Day2()
{
    var resultPart1 = 0;
    var resultPart2 = 0;
    var input = File.ReadAllLines(@"Inputs\Day2.txt");
    foreach (var item in input)
    {
        var values = item.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
        var isIncreasing = values[0] < values[1];
        var noToleranceResult = IsValidReport(values, isIncreasing);
        resultPart1 += noToleranceResult;
        resultPart2 += noToleranceResult == 1 ? 1 : IsValidReportFaultTolerated(values);
    }
    WriteResult(2, 1, $"{resultPart1}");
    WriteResult(2, 2, $"{resultPart2}");
}

static int IsValidReportFaultTolerated(int[] values)
{
    // Try validation by removing values
    for (int i = 0; i < values.Length; i++)
    {
        var valuesToCheck = values.ToList();
        valuesToCheck.RemoveAt(i);
        var isIncreasing = valuesToCheck[0] < valuesToCheck[1];
        if (IsValidReport(valuesToCheck.ToArray(), isIncreasing) == 1)
        {
            // Console.WriteLine($"Right {string.Join(',', valuesToCheck)} - Based on {string.Join(',', values)}");
            return 1;
        }

    }
    // Console.WriteLine($"False {string.Join(',', values)}");
    return 0;
}

static int IsValidReport(int[] values, bool isIncreasing)
{
    foreach (var (index, value) in values.Index().Skip(1))
    {
        // If isIncreasing but the value is smaller then the one before the report isn't valid
        if (isIncreasing)
        {
            if (value <= values[index - 1])
                return 0;
        }
        // Decreasing
        else
        {
            if (value >= values[index - 1])
                return 0;
        }
        // If increasing / decreasing is right, check for the difference
        var difference = Math.Abs(value - values[index - 1]);
        if (difference < 1 || difference > 3)
            return 0;
    }
    return 1;
}

static void Day1()
{
    var input = File.ReadAllLines(@"Inputs\Day1.txt");
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
    WriteResult(1, 1, $"{resultPart1}");

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

    WriteResult(1, 2, $"{resultPart2}");

}

static void WriteResult(int day, int part, string result)
{
    Console.WriteLine($"Result day {day}, part {part}: {result}");
    Console.WriteLine("Press enter to continue");
    Console.ReadLine();
}