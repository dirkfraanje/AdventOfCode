using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

//Day1();
//Day2();
Day3();

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