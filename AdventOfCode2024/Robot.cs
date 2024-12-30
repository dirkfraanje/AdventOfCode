using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2024
{
    internal class Robot : CoordinateBase
    {
        public Robot(int x, int y, int velocityX, int velocityY) : base(x, y)
        {
            VelocityX = velocityX;
            VelocityY = velocityY;
        }
        public int VelocityX { get; private set; }
        public int VelocityY { get; private set; }

        public void SetFinalPosition(int seconds, int xTileSize, int yTileSize)
        {
            for (int i = 0; i < seconds; i++)
            {
                // Positive x means the robot is moving to the right,
                // and positive y means the robot is moving down.
                // p=2,4 v=2,-3
                // 4, 1
                // 6, 5
                X = (xTileSize + (X + VelocityX)) % xTileSize;
                Y = (yTileSize + (Y + VelocityY)) % yTileSize;
            }
        }
    }
}
