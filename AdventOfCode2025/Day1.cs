using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025
{
    internal class Day1
    {
        public static void Result(bool includeRotations)
        {
            var input = File.ReadAllLines(@"Inputs\Day1.txt");
            var pointer = 50;
            var password = 0;
            foreach (var line in input)
            {
                var direction = line[0];
                var steps = int.Parse(line[1..]);

                if (direction == 'L')
                {
                    pointer -= steps;
                    while (pointer < -99)
                    {
                        // If pointer + steps is 0 we have alread counted this at if(pointer == 0)
                        if (pointer + steps != 0 && includeRotations)
                        {
                            //Console.WriteLine(line);
                            password++;
                        }

                        // Still not in range
                        pointer += 100;
                    }
                    if (pointer < 0)
                    {
                        if (pointer + steps != 0 && includeRotations)
                        {
                            //Console.WriteLine(line);
                            password++;
                        }

                        pointer = pointer + 100;

                    }

                }
                else if (direction == 'R')
                {
                    pointer += steps;
                    while (pointer > 99)
                    {
                        if (pointer != 100 && includeRotations)
                        {
                            //Console.WriteLine(line);
                            password++;
                        }
                        // Still not in range
                        pointer -= 100;
                    }
                }
                else
                    throw new Exception($"Unknown direction: {direction}");
                if (pointer == 0)
                {
                    //Console.WriteLine(line);
                    password++;
                }

            }
            var part = includeRotations ? 2 : 1;
            Console.WriteLine($"Result part{part}:  {password}");
        }
    }
}
