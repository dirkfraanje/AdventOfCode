using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2025
{
    public class Day2
    {
        public static void ResultPart1()
        {
            long result = 0;
            var input = File.ReadAllText(@"Inputs\Day2.txt").Split(',');

            foreach (var item in input)
            {
                var ids = item.Split('-');
                var startProductId = long.Parse(ids[0]);
                var endProductId = long.Parse(ids[1]);
                for (; startProductId <= endProductId; startProductId++)
                {
                    var productId = startProductId.ToString();
                    var totalDigits = productId.Length;
                    // No match possible if it's uneven
                    if (totalDigits % 2 != 0)
                        continue;

                    var halfTotal = totalDigits / 2;

                    var leftPart = productId.Substring(0, halfTotal);
                    var rightPart = productId.Substring(halfTotal);
                    if (leftPart.Equals(rightPart))
                        result += startProductId;
                }
            }

            Console.WriteLine($"Result part 1: {result}");
        }

        public static void ResultPart2()
        {
            long result = 0;
            var input = File.ReadAllText(@"Inputs\Day2.txt").Split(',');

            foreach (var item in input)
            {
                var ids = item.Split('-');
                var startProductId = long.Parse(ids[0]);
                var endProductId = long.Parse(ids[1]);
                for (; startProductId <= endProductId; startProductId++)
                {
                    long invalidId = GetInvalidId(startProductId);
                    if ((invalidId > 0))
                    {
                        result += invalidId;
                    }
                }
            }

            Console.WriteLine($"Result part 2: {result}");
        }

        private static long GetInvalidId(long startProductId)
        {
            // 1 1111
            // 12 1212
            // 123 123
            // 1234 1234 
            // etc
            // Start checking repeations: first 2 repeated? first 3 repeated? etc
            var productid = startProductId.ToString();

            var equal = false;
            // Value to compare to
            for (int i = 1; i <= productid.Length / 2; i++)
            {
                
                var compareToValue = productid.Substring(0, i);
                // Remove the comparing value
                productid = productid.Substring(i);
                while (true)
                {
                    if (productid.Length == 0)
                    {
                        if(equal)
                            return startProductId;
                        break;
                    }

                    // If the length of the remainder product id mismatches i break , for example: 21213, comparing 21 with 21 and then 3 remains
                    if(productid.Length < i)
                    {
                        equal = false;
                        // Reset productid
                        productid = startProductId.ToString();
                        break;
                    }


                    // Remove the comparing value
                    var productidToCheck = productid.Substring(0, i);
                    // Remove the value that will be checked value
                    productid = productid.Substring(i);

                    // Compare
                    if (productidToCheck.Equals(compareToValue))
                    {
                        equal = true;
                        continue;
                    }
                    else
                    {
                        equal = false;
                        // Reset productid
                        productid = startProductId.ToString();
                        break;
                    }
                }
            }
            
            return -1;
        }
    }
}
