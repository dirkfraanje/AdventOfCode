using AdventOfCode2024;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

//Day1();
//Day2();
//Day3();
//await Day4();
//Day5();
Day6();

async void Day6()
{
    var input = File.ReadAllLines(@"Inputs\Day6.txt");

    var startingMap = new List<Coordinate>();

    for (int y = 0; y < input.Length; y++)
    {
        var line = input[y];
        for (int x = 0; x < line.Length; x++)
        {
            startingMap.Add(new Coordinate(x, y, line[x]));
        }
    }
    #region Part1
    var mapPart1 = new List<Coordinate>(startingMap);
    var currentLocation = mapPart1.First(c => c.Value.Equals('^') || c.Value.Equals('>') || c.Value.Equals('v') || c.Value.Equals('<'));
    var currentDirection = currentLocation.Value.Equals('^') ? Direction.North :
        currentLocation.Value.Equals('>') ? Direction.East :
        currentLocation.Value.Equals('v') ? Direction.South :
        Direction.West;
    Coordinate nextLocation;
    currentLocation.Visited = true;
    var insideMap = true;
    while (insideMap)
    {

        switch (currentDirection)
        {
            case Direction.North:
                nextLocation = mapPart1.FirstOrDefault(c => c.X == currentLocation.X && c.Y == currentLocation.Y - 1);
                if (nextLocation == null)
                {
                    insideMap = false;
                    break;
                }

                if (nextLocation.Value.Equals('#'))
                    currentDirection = Direction.East;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            case Direction.East:
                nextLocation = mapPart1.FirstOrDefault(c => c.X == currentLocation.X + 1 && c.Y == currentLocation.Y);
                if (nextLocation == null)
                    if (nextLocation == null)
                    {
                        insideMap = false;
                        break;
                    }
                if (nextLocation.Value.Equals('#'))
                    currentDirection = Direction.South;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            case Direction.South:
                nextLocation = mapPart1.FirstOrDefault(c => c.X == currentLocation.X && c.Y == currentLocation.Y + 1);
                if (nextLocation == null)
                    if (nextLocation == null)
                    {
                        insideMap = false;
                        break;
                    }
                if (nextLocation.Value.Equals('#'))
                    currentDirection = Direction.West;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            case Direction.West:
                nextLocation = mapPart1.FirstOrDefault(c => c.X == currentLocation.X - 1 && c.Y == currentLocation.Y);
                if (nextLocation == null)
                    if (nextLocation == null)
                    {
                        insideMap = false;
                        break;
                    }
                if (nextLocation.Value.Equals('#'))
                    currentDirection = Direction.North;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            default:
                break;
        }
    }
    WriteResult(6, 1, $"{mapPart1.Count(x => x.Visited)}");
    Console.WriteLine("Part 2 while take a while...");
    #endregion

    #region Part2
    // For each coordinate with value . check if a loop is created if we set an obstruction here

    var currentLocation2 = startingMap.First(c => c.Value.Equals('^') || c.Value.Equals('>') || c.Value.Equals('v') || c.Value.Equals('<'));
    var currentDirection2 = currentLocation2.Value.Equals('^') ? Direction.North :
        currentLocation.Value.Equals('>') ? Direction.East :
        currentLocation.Value.Equals('v') ? Direction.South :
        Direction.West;
    var tasks = new List<Task<bool>>();
    foreach (var item in startingMap.Where(x => x.Visited && x.Value.Equals('.')).ToArray())
    {
        tasks.Add(goingToLoopIfIAmObstructing(startingMap, item, currentLocation2, currentDirection2));

    }
    var resultList = await Task.WhenAll(tasks);
    #endregion
    WriteResult(6, 2, $"{resultList.Count(x => x)}");

}

async Task<bool> goingToLoopIfIAmObstructing(List<Coordinate> map, Coordinate iAmObstructing, Coordinate currentLocation, Direction currentDirection)
{
    // Let's keep a list of visited locations
    Console.WriteLine($"Checking location x:{iAmObstructing.X} y:{iAmObstructing.Y} ");
    Coordinate nextLocation;
    var visitedLocations = new Dictionary<Coordinate, List<Direction>>();
    var insideMap = true;
    while (insideMap)
    {

        switch (currentDirection)
        {
            case Direction.North:
                nextLocation = map.FirstOrDefault(c => c.X == currentLocation.X && c.Y == currentLocation.Y - 1);
                if (nextLocation == null)
                {
                    // Even I am also obstructing the guard still get; out of the loop
                    return false;
                }
                else
                {
                    if (CheckLooping(currentDirection, nextLocation, visitedLocations))
                        return true;
                }


                if (nextLocation.Value.Equals('#') || nextLocation == iAmObstructing)
                    currentDirection = Direction.East;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            case Direction.East:
                nextLocation = map.FirstOrDefault(c => c.X == currentLocation.X + 1 && c.Y == currentLocation.Y);
                if (nextLocation == null)
                {
                    // Even I am also obstructing the guard still get; out of the loop
                    return false;
                }
                else
                {
                    if (CheckLooping(currentDirection, nextLocation, visitedLocations))
                        return true;
                }


                if (nextLocation.Value.Equals('#') || nextLocation == iAmObstructing)
                    currentDirection = Direction.South;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            case Direction.South:
                // Find the next location
                nextLocation = map.FirstOrDefault(c => c.X == currentLocation.X && c.Y == currentLocation.Y + 1);
                // If there is no next location the guard get's of the map
                if (nextLocation == null)
                    return false;
                // There is a next location
                else
                {
                    if (CheckLooping(currentDirection, nextLocation, visitedLocations))
                        return true;
                }


                if (nextLocation.Value.Equals('#') || nextLocation == iAmObstructing)
                    currentDirection = Direction.West;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            case Direction.West:
                // Find the next location
                nextLocation = map.FirstOrDefault(c => c.X == currentLocation.X - 1 && c.Y == currentLocation.Y);
                // If there is no next location the guard get's of the map
                if (nextLocation == null)
                    return false;
                // There is a next location
                else
                {
                    if (CheckLooping(currentDirection, nextLocation, visitedLocations))
                        return true;
                }

                if (nextLocation.Value.Equals('#') || nextLocation == iAmObstructing)
                    currentDirection = Direction.North;
                else
                {
                    nextLocation.Visited = true;
                    currentLocation = nextLocation;
                }
                break;
            default:
                break;
        }
    }
    return false;
}

void Day5()
{
    var input = File.ReadAllLines(@"Inputs\Day5.txt");

    var creatingOrderingRules = true;
    List<(int First, int Second)> orderingRules = new();
    List<string> succesRules = new List<string>();
    List<string> faultRules = new List<string>();
    foreach (var line in input)
    {

        if (string.IsNullOrWhiteSpace(line))
            creatingOrderingRules = false;

        if (creatingOrderingRules)
        {
            var numbers = line.Split('|');
            orderingRules.Add((int.Parse(numbers[0]), int.Parse(numbers[1])));
        }

        if (!creatingOrderingRules && !string.IsNullOrEmpty(line))
        {
            if (UpdateSucces(line, orderingRules))
                // If the loop get's here no faults have been found
                succesRules.Add(line);
            else
                faultRules.Add(line);
        }
    }
    var result = 0;
    foreach (var line in succesRules)
    {
        var numbers = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
        var middle = (numbers.Count() / 2);
        result += numbers[middle];
    }
    WriteResult(5, 1, $"{result}");

    var result2 = 0;
    // The right order is 97 75 47 61 53 29 13 but in the real puzzle there can be conflicting rules
    // So for each update the right order has to be found, only based on rules that contain values that are in the update
    foreach (var faultRule in faultRules)
    {
        var faultedNumbers = faultRule.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x));
        List<(int First, int Second)> orderingRulesWithFaultValues = new List<(int, int)>();

        foreach (var item in orderingRules)
        {
            if (faultedNumbers.Contains(item.First) && faultedNumbers.Contains(item.Second))
                orderingRulesWithFaultValues.Add(item);
        }

        List<int> rightOrderedRuleValues = new();
        foreach (var rule in orderingRulesWithFaultValues.ToArray())
        {
            // Set initial values
            if (rightOrderedRuleValues.Count == 0)
            {
                rightOrderedRuleValues.Add(rule.First);
                rightOrderedRuleValues.Add(rule.Second);
                continue;
            }
            // Place first value
            var valueToSet = rule.First;
            var checkList = new List<int>(rightOrderedRuleValues);

            var valid = false;
            var index = 0;
            while (!valid)
            {
                if (rightOrderedRuleValues.Contains(valueToSet))
                    break;
                if (index > checkList.Count)
                    checkList.Add(index);
                else
                    checkList.Insert(index, valueToSet);
                valid = UpdateSuccesCheck(checkList, orderingRules);
                if (!valid)
                {
                    checkList.Remove(rule.First);
                    index++;
                }
                else
                    rightOrderedRuleValues.Insert(index, valueToSet);
            }
            valid = false;
            index = 0;
            valueToSet = rule.Second;
            // Place second value
            while (!valid)
            {
                if (rightOrderedRuleValues.Contains(valueToSet))
                    break;
                if (index > checkList.Count)
                    checkList.Add(valueToSet);
                else
                    checkList.Insert(index, valueToSet);
                valid = UpdateSuccesCheck(checkList, orderingRules);
                if (!valid)
                {
                    checkList.Remove(rule.Second);
                    index++;
                }
                else
                    rightOrderedRuleValues.Insert(index, valueToSet);
            }
        }
        //Console.WriteLine(faultRule);
        //WriteResult(5, 2, $"{string.Join(',', rightOrderedRuleValues)}");
        var middle = (rightOrderedRuleValues.Count() / 2);
        result2 += rightOrderedRuleValues[middle];
    }


    WriteResult(5, 2, $"{result2}");

}

bool UpdateSuccesCheck(List<int> numbers, List<(int First, int Second)> orderingRules)
{
    for (int i = 0; i < numbers.Count - 1; i++)
    {
        //Console.WriteLine($"i:{numbers[i]}");
        var number = numbers[i];
        for (int j = i + 1; j < numbers.Count; j++)
        {
            //Console.WriteLine($"j:{numbers[j]}");
            var orderingRuleFaulting = orderingRules.FirstOrDefault(r => r.First.Equals(numbers[j]) && r.Second.Equals(numbers[i]));
            if (orderingRuleFaulting.First != 0)
                return false;
        }

    }
    return true;
}

bool UpdateSucces(string line, List<(int First, int Second)> orderingRules)
{
    var numbers = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToArray();
    for (int i = 0; i < numbers.Length - 1; i++)
    {
        //Console.WriteLine($"i:{numbers[i]}");
        var number = numbers[i];
        for (int j = i + 1; j < numbers.Length; j++)
        {
            //Console.WriteLine($"j:{numbers[j]}");
            if (orderingRules.Any(r => r.First.Equals(numbers[j]) && r.Second.Equals(numbers[i])))
                return false;
        }

    }
    return true;
}
async Task Day4()
{

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
                grid.Add(new Coordinate(x, y, letter));
                resultLine.Append($"{green}{letter}{normal}");
            }

            else
            {
                grid.Add(new Coordinate(x, y, letter));
                resultLine.Append($"{normal}{letter}{normal}");
            }

        }
        //Console.WriteLine(resultLine.ToString());
    }

    List<Task<int>> tasks1 = new List<Task<int>>();
    foreach (var item in grid.Where(c => c.Value.Equals('X')))
    {
        tasks1.Add(CheckXMASResults(grid, item));
    }
    var resultFromTask1 = await Task.WhenAll(tasks1);
    WriteResult(4, 1, $"{resultFromTask1.Sum()}");

    // Part 2
    List<Task<int>> tasks2 = new List<Task<int>>();
    foreach (var item in grid.Where(c => c.Value.Equals('M') || c.Value.Equals('S')))
    {
        tasks2.Add(CheckForMASResults(grid, item));
    }
    var resultFromTasks2 = await Task.WhenAll(tasks2);
    WriteResult(4, 2, $"{resultFromTasks2.Sum()}");
}

async Task<int> CheckForMASResults(List<Coordinate> grid, Coordinate item)
{
    // We always receive the top left starting point, either M or S
    // From this starting point we are checking the diagonal right/bottom
    // If this is M we check for A and S, otherwise for A and M
    if (item.Value.Equals('M'))
    {
        if (!CheckForAS(grid, item.X + 1, item.Y + 1, item.X + 2, item.Y + 2))
            return 0;
    }
    else
    {
        if (!CheckForAM(grid, item.X + 1, item.Y + 1, item.X + 2, item.Y + 2))
            return 0;
    }

    // Next we check if the value on the X coordinate 2 position further is either M or S
    var position2 = grid.FirstOrDefault(c => c.X == item.X + 2 && c.Y.Equals(item.Y));
    if (!(position2.Value == 'M' || position2.Value == 'S'))
        return 0;
    if (position2.Value.Equals('M'))
    {
        if (!CheckForAS(grid, position2.X - 1, position2.Y + 1, position2.X - 2, position2.Y + 2))
            return 0;
    }
    else
    {
        if (!CheckForAM(grid, position2.X - 1, position2.Y + 1, position2.X - 2, position2.Y + 2))
            return 0;
    }
    return 1;
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
bool CheckForAM(List<Coordinate> grid, int x1, int y1, int x2, int y2)
{
    var valueM = grid.FirstOrDefault(c => c.X == x1 && c.Y == y1);
    if (valueM.Value != 'A')
        return false;

    var valueA = grid.FirstOrDefault(c => c.X == x2 && c.Y == y2);
    if (valueA.Value != 'M')
        return false;

    return true;
}
bool CheckForAS(List<Coordinate> grid, int x1, int y1, int x2, int y2)
{
    var valueM = grid.FirstOrDefault(c => c.X == x1 && c.Y == y1);
    if (valueM.Value != 'A')
        return false;

    var valueA = grid.FirstOrDefault(c => c.X == x2 && c.Y == y2);
    if (valueA.Value != 'S')
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

bool CheckLooping(Direction currentDirection, Coordinate nextLocation, Dictionary<Coordinate, List<Direction>> visitedLocations)
{
    // If this location already exists in the list of visited locations we have to check if it's looping
    var exists = visitedLocations.TryGetValue(nextLocation, out var directions);
    if (exists)
    {
        // If this location has been visited before from this direction we now it's looping
        if (directions.Any(x => x == currentDirection))
            return true;
    }
    // If it doesn't exist we add it to the visited locations
    else
    {
        if (directions == null)
            visitedLocations.Add(nextLocation, new List<Direction>() { currentDirection });
        else
            directions.Add(currentDirection);

    }
    return false;
}