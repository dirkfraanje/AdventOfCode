using AdventOfCode2024;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipes;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

// Day1();
// Day2();
// Day3();
// await Day4();
// Day5();
// await Day6();
// Day7();
// Day8();
// Day9();
// Day10();
Day11(1, 25);
Day11(2, 75);
void Day11(int part, int times)
{
    long finalResult = 0;


    // Let's create a dictionary of tuple: 
    Dictionary<(long zeroToNine, int timesLeft), long> calculationResult = new();
    // Start from zero times
    for (int timesLeft = 1; timesLeft < times; timesLeft++)
    {
        for (int zeroToNine = 0; zeroToNine < 10; zeroToNine++)
        {
            // So get the amount of stones for each value from 0 to 9 
            calculationResult.Add((zeroToNine, timesLeft), GetStones(zeroToNine, timesLeft, calculationResult));
        }
    }
    var input = "125 17".Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
    foreach (var x in input)
    {
        finalResult += GetStones(x, times, calculationResult);
    }

    WriteResult(11, part, $"{finalResult}");
}

long GetStones(long item, int timesLeft, Dictionary<(long zeroToNine, int timesLeft), long> calculationResult)
{
    if (timesLeft <= 0)
        return 1;
    (bool keyExists, long value) = calculationResult.TryGetValue((item, timesLeft), out var val) ? (true, val) : (false, val);
    if (keyExists)
    {
        return value;
    }
    if (item == 0)
        return GetStones(1, timesLeft - 1, calculationResult);

    else if ($"{item}".Count() % 2 == 0)
    {
        var numberOfDigits = $"{item}".Count();
        long subResult = 0;
        var value1 = long.Parse($"{item}".Substring(0, numberOfDigits / 2));
        var value2 = long.Parse($"{item}".Substring($"{item}".Count() / 2));
        subResult += GetStones(value1, timesLeft - 1, calculationResult);
        subResult += GetStones(value2, timesLeft - 1, calculationResult);
        return subResult;
    }
    else
        return GetStones(item * 2024, timesLeft - 1, calculationResult);
}


void Day10()
{
    var map = new List<TrailCoordinate>();
    var input = ReadInput(10);
    // Create map
    for (int y = 0; y < input.Length; y++)
    {
        var line = input[y];
        for (int x = 0; x < line.Length; x++)
        {
            map.Add(new TrailCoordinate(x, y, (int)char.GetNumericValue(line[x]), map));
        }
    }
    //      10..9..
    //      2...8..
    //      3...7..
    //      4567654
    //      ...8..3
    //      ...9..2
    //      .....01

    // Top has 1
    // below 2
    // Count trailheads
    foreach (var trailHead in map.Where(x => x.Height == 0))
    {
        trailHead.EndsItCanReach(trailHead);
    }


    WriteResult(10, 1, $"{map.Sum(t => t.ReachedBy.Count)}");
    WriteResult(10, 2, $"{map.Sum(t => t.ReachedByTotal)}");
}

void Day9()
{
    var input = ReadInput(9)[0].Select(x => (int)char.GetNumericValue(x)).ToArray();
    List<DiskFragment> fragments = new();
    var count = input.Count();
    var id = 0;
    for (int i = 0; i < count; i++)
    {
        // Block size
        var blockSize = input[i];
        // Free space
        int freeSpaceSize = 0;
        i++;
        if (i < count)
            freeSpaceSize = input[i];
        var ids = new List<int>();
        // Default all to -1
        for (int j = 0; j < blockSize + freeSpaceSize; j++)
        {
            ids.Add(-1);
        }
        var index = 0;
        for (int j = 0; j < blockSize; j++)
        {
            ids[j] = id;
        }


        fragments.Add(new DiskFragment(ids, id));
        id++;
    }
    // Start defragmenting
    // Begin at the end
    var fragmentsReversed = new List<DiskFragment>(fragments);
    fragmentsReversed.Reverse();
    foreach (var item in fragmentsReversed)
    {
        var enoughSpace = GetFragmentWithEnoughSpace(fragments, item);
        // Enough space should not greater than the position of item because then it will move it from left to right and it should just move from right tot left
        if (fragments.IndexOf(enoughSpace) >= fragments.IndexOf(item))
            continue;
        if (enoughSpace != null)
        {
            var lastIdCheckWith = item.FileIds.FirstOrDefault(x => x.Equals(item.OriginalId));
            while (true)
            {
                var lastId = item.FileIds.FirstOrDefault(x => x.Equals(item.OriginalId));
                if (lastId != lastIdCheckWith)
                    break;
                var firstFreeSpace = enoughSpace.FileIds.IndexOf(-1);

                enoughSpace.FileIds[firstFreeSpace] = lastId;

                var position = item.FileIds.LastIndexOf(lastId);
                item.FileIds[position] = -1;

            }

        }
    }

    var result = fragments.SelectMany(x => x.FileIds).ToArray();
    long checkSum = 0;
    for (int i = 0; i < result.Count(); i++)
    {
        var before = checkSum;
        if (result[i] != -1)
            checkSum += i * result[i];
        Console.WriteLine($"{before} += {i} * {result[i]} = {checkSum}");
    }

    WriteResult(9, 2, $"{checkSum}");

    #region Part 1
    // Start defragmenting
    // Begin at the end     
    //while (true)
    //{
    //    // as long as there is a fragment that has free space && has fragments after it also has free space keep going
    //    var firstWithFreeSpace = fragments.FirstOrDefault(x => x.FileIds.Any(fileid => fileid.Equals(-1) && x.FileIds.Any(fileid => !fileid.Equals(-1))));
    //    if (firstWithFreeSpace == null)
    //        break;
    //    var valueToCheckFrom2 = fragments.LastOrDefault(x => x != firstWithFreeSpace && x.FileIds.Any(fileid => fileid.Equals(-1) && x.FileIds.Any(fileid => !fileid.Equals(-1))));
    //    if (valueToCheckFrom2 == null)
    //        break;

    //    var lastWithIds = fragments.LastOrDefault(x => x.FileIds.Any(id => id != -1));
    //    var lastId = lastWithIds.FileIds.First(x => !x.Equals(-1));
    //    //var lastPositionWithId = lastWithIds.FileIds.LastIndexOf(lastId);

    //    var firstFreeSpace = firstWithFreeSpace.FileIds.IndexOf(-1);

    //    firstWithFreeSpace.FileIds[firstFreeSpace] = lastId;
    //    // set to -1 the one that is removed
    //    var position = lastWithIds.FileIds.LastIndexOf(lastId);
    //    lastWithIds.FileIds[position] = -1;
    //}
    //var result = fragments.SelectMany(x => x.FileIds.Where(x => !x.Equals(-1))).ToArray();
    //long checkSum = 0;
    //for (int i = 0; i < result.Count(); i++)
    //{
    //    checkSum += i * result[i];
    //}

    //WriteResult(9, 1, $"{checkSum}");
    #endregion
}

DiskFragment GetFragmentWithEnoughSpace(List<DiskFragment> fragments, DiskFragment item)
{
    var needed = item.FileIds.Count(x => x.Equals(item.OriginalId));
    return fragments.FirstOrDefault(x => x.FileIds.Count(i => i == -1) >= needed);
}

void Day8()
{
    var input = ReadInput(8);

    var startingMap = new List<Coordinate>();

    for (int y = 0; y < input.Length; y++)
    {
        var line = input[y];
        for (int x = 0; x < line.Length; x++)
        {
            startingMap.Add(new Coordinate(x, y, line[x]));
        }
    }

    foreach (var locationValue in startingMap.Where(l => Char.IsAsciiLetterOrDigit(l.Value)).GroupBy(l => l.Value).Select(l => l.Key))
    {
        var antennasSameFrequency = startingMap.Where(a => a.Value.Equals(locationValue)).ToList();
        antennasSameFrequency.ForEach(x => x.HasAntinode = true);
        // Start forming pairs
        // a1 and a2
        // a1 and a3
        // a1 and a4
        // a2 and a3
        // a2 and a4
        // a3 and a4
        while (antennasSameFrequency.Count > 1)
        {
            var checkingAntenna = antennasSameFrequency.First();
            antennasSameFrequency.Remove(checkingAntenna);
            foreach (var antenna in antennasSameFrequency)
            {
                // Detect the location of the 2 antinodes, if they are on the map enable has antinode
                // x8 y1 x5 y2
                // x8 y1 x7 y3
                // x8 y1 x4 y4
                // x5 y2 x7 y3
                // x5 y2 x4 y4
                // x7 y3 x4 y4

                var differenceX = Math.Abs(checkingAntenna.X - antenna.X);
                var differenceY = Math.Abs(checkingAntenna.Y - antenna.Y);
                Coordinate upperAntiNode;
                Coordinate lowerAntiNode;
                // Upper antinode can either be on the left or on the right or directly above
                // Directly above
                //if (checkingAntenna.X == antenna.X)
                //{
                //    upperAntiNode = GetAntiNode(startingMap, checkingAntenna.X, checkingAntenna.Y - differenceY);
                //    lowerAntiNode = GetAntiNode(startingMap, checkingAntenna.X, antenna.Y + differenceY);
                //}
                // The upper antenna is on the right
                //else 
                if (checkingAntenna.X > antenna.X)
                {
                    upperAntiNode = GetAntiNode(startingMap, checkingAntenna.X + differenceX, checkingAntenna.Y - differenceY);
                    GetUpperAntiNodesToEndOfMapUpperRight(startingMap, checkingAntenna, differenceX, differenceY);

                    lowerAntiNode = GetAntiNode(startingMap, antenna.X - differenceX, antenna.Y + differenceY);
                    GetLowerAntiNodesToEndOfMapUpperRight(startingMap, antenna, differenceX, differenceY);

                }
                // The upper is on the left
                else if (antenna.X > checkingAntenna.X)
                {
                    upperAntiNode = GetAntiNode(startingMap, checkingAntenna.X - differenceX, checkingAntenna.Y - differenceY);
                    GetUpperAntiNodesToEndOfMapUpperLeft(startingMap, antenna, differenceX, differenceY);

                    lowerAntiNode = GetAntiNode(startingMap, antenna.X + differenceX, antenna.Y + differenceY);
                    GetLowerAntiNodesToEndOfMapUpperLeft(startingMap, checkingAntenna, differenceX, differenceY);
                }
                else
                    throw new Exception("Something weird is going on");
                if (upperAntiNode != null)
                    upperAntiNode.HasAntinodeOnly1 = true;



                if (lowerAntiNode != null)
                    lowerAntiNode.HasAntinodeOnly1 = true;

            }
        }


    }
    for (int y = 0; y < 12; y++)
    {
        var line = "";
        for (int x = 0; x < 12; x++)
        {
            var location = startingMap.First(l => l.X == x && l.Y == y);
            if (location.HasAntinode)
                line += "#";
            else
                line += location.Value;

        }
        Console.WriteLine(line);
    }
    WriteResult(8, 1, $"{startingMap.Count(l => l.HasAntinodeOnly1)}");
    WriteResult(8, 2, $"{startingMap.Count(l => l.HasAntinode)}");

}

void GetLowerAntiNodesToEndOfMapUpperRight(List<Coordinate> startingMap, Coordinate antenna, int differenceX, int differenceY)
{
    Coordinate nextAntiNode;
    do
    {
        nextAntiNode = GetAntiNode(startingMap, antenna.X - differenceX, antenna.Y + differenceY);
        if (nextAntiNode != null)
        {
            nextAntiNode.HasAntinode = true;
            antenna = nextAntiNode;
        }
    } while (nextAntiNode != null);
}

void GetUpperAntiNodesToEndOfMapUpperRight(List<Coordinate> startingMap, Coordinate checkingAntenna, int differenceX, int differenceY)
{
    Coordinate nextAntiNode;
    do
    {
        nextAntiNode = GetAntiNode(startingMap, checkingAntenna.X + differenceX, checkingAntenna.Y - differenceY);
        if (nextAntiNode != null)
        {
            nextAntiNode.HasAntinode = true;
            checkingAntenna = nextAntiNode;
        }
    } while (nextAntiNode != null);
}

void GetLowerAntiNodesToEndOfMapUpperLeft(List<Coordinate> startingMap, Coordinate checkingAntenna, int differenceX, int differenceY)
{
    Coordinate nextAntiNode;
    do
    {
        nextAntiNode = GetAntiNode(startingMap, checkingAntenna.X - differenceX, checkingAntenna.Y - differenceY);
        if (nextAntiNode != null)
        {
            nextAntiNode.HasAntinode = true;
            checkingAntenna = nextAntiNode;
        }
    } while (nextAntiNode != null);
}

void GetUpperAntiNodesToEndOfMapUpperLeft(List<Coordinate> startingMap, Coordinate antenna, int differenceX, int differenceY)
{
    Coordinate nextAntiNode;
    do
    {
        nextAntiNode = GetAntiNode(startingMap, antenna.X + differenceX, antenna.Y + differenceY);
        if (nextAntiNode != null)
        {
            nextAntiNode.HasAntinode = true;
            antenna = nextAntiNode;
        }
    } while (nextAntiNode != null);
}


Coordinate GetAntiNode(List<Coordinate> startingMap, int x, int y)
{
    return startingMap.FirstOrDefault(l => l.X == x && l.Y == y);
}

void Day7()
{
    var input = ReadInput(7);
    long result = 0;
    long result2 = 0;
    foreach (var equation in input)
    {
        result += CanBeResolved(equation);
        result2 += CanBeResolved2(equation);
    }
    WriteResult(7, 1, $"{result}");
    WriteResult(7, 2, $"{result2}");

}

long CanBeResolved2(string equation)
{
    var testValueAndNumbers = equation.Split(':', StringSplitOptions.RemoveEmptyEntries);
    var testValue = long.Parse(testValueAndNumbers[0]);
    var numbers = testValueAndNumbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
    var value = numbers.First();
    List<long> calculateOn = new List<long>();
    calculateOn.Add(value);
    numbers.Remove(value);
    GetResults2(numbers, calculateOn);

    // 292: 11 6 16 20
    // 11 + 6 + 16 + 20
    // 11 * 6 + 16 + 20
    // 11 + 6 * 16 + 20
    // 11 + 6 + 16 * 20
    // 11 * 6 * 16 + 20
    // 11 * 6 * 16 * 20

    // 11 + 6
    // 11 * 6

    // 17 + 16
    // 17 * 17
    // 66 + 16
    // 66 * 16
    if (calculateOn.Contains(testValue))
        return testValue;
    return 0;
}

long CanBeResolved(string equation)
{
    var testValueAndNumbers = equation.Split(':', StringSplitOptions.RemoveEmptyEntries);
    var testValue = long.Parse(testValueAndNumbers[0]);
    var numbers = testValueAndNumbers[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
    var value = numbers.First();
    List<long> calculateOn = new List<long>();

    calculateOn.Add(value);
    numbers.Remove(value);
    GetResults(numbers, calculateOn);

    // 292: 11 6 16 20
    // 11 + 6 + 16 + 20
    // 11 * 6 + 16 + 20
    // 11 + 6 * 16 + 20
    // 11 + 6 + 16 * 20
    // 11 * 6 * 16 + 20
    // 11 * 6 * 16 * 20

    // 11 + 6
    // 11 * 6

    // 17 + 16
    // 17 * 17
    // 66 + 16
    // 66 * 16
    if (calculateOn.Contains(testValue))
        return testValue;
    return 0;
}
List<long> GetResults2(List<long> numbers, List<long> calculateOn)
{
    var numberToCalculateWith = numbers.First();
    numbers.Remove(numberToCalculateWith);
    var calculateOnStore = calculateOn.ToArray();
    calculateOn.Clear();
    foreach (var number in calculateOnStore)
    {
        calculateOn.Add(number * numberToCalculateWith);
        calculateOn.Add(number + numberToCalculateWith);

        calculateOn.Add(long.Parse($"{number}{numberToCalculateWith}"));
    }

    // do calculations
    if (numbers.Count > 0)
    {
        return GetResults2(numbers, calculateOn);
    }
    return numbers;
}
List<long> GetResults(List<long> numbers, List<long> calculateOn)
{
    var numberToCalculateWith = numbers.First();
    numbers.Remove(numberToCalculateWith);
    var calculateOnStore = calculateOn.ToArray();
    calculateOn.Clear();
    foreach (var number in calculateOnStore)
    {
        calculateOn.Add(number * numberToCalculateWith);
        calculateOn.Add(number + numberToCalculateWith);
    }

    // do calculations
    if (numbers.Count > 0)
    {
        return GetResults(numbers, calculateOn);
    }
    return numbers;
}

string[] ReadInput(int day)
{
    return File.ReadAllLines(@$"Inputs\Day{day}.txt");
}
async Task Day6()
{
    var input = ReadInput(6);

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

    var resultList = new ConcurrentBag<bool>(); // Thread-safe collection for results.

    await Parallel.ForEachAsync(
        startingMap.Where(x => x.Visited && x.Value.Equals('.')),
        new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, // Adjust concurrency if needed.
        async (item, cancellationToken) =>
        {
            var result = await goingToLoopIfIAmObstructing(startingMap, item, currentLocation2, currentDirection2);
            resultList.Add(result);
        });

    //var tasks = new List<Task<bool>>();
    //foreach (var item in startingMap.Where(x => x.Visited && x.Value.Equals('.')).ToArray())
    //{
    //    tasks.Add(goingToLoopIfIAmObstructing(startingMap, item, currentLocation2, currentDirection2));

    //}
    //var resultList = await Task.WhenAll(tasks);
    #endregion
    WriteResult(6, 2, $"{resultList.Count(x => x)}");

}

async Task<bool> goingToLoopIfIAmObstructing(List<Coordinate> map, Coordinate iAmObstructing, Coordinate currentLocation, Direction currentDirection)
{
    // Let's keep a list of visited locations
    //Console.WriteLine($"Checking location x:{iAmObstructing.X} y:{iAmObstructing.Y} ");
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
    var input = ReadInput(5);

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
    var input = ReadInput(4);
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

void Day2()
{
    var resultPart1 = 0;
    var resultPart2 = 0;
    var input = ReadInput(2);
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

void Day1()
{
    var input = ReadInput(1);
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