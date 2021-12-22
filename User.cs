using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppGameCrossZero
{
    class User : Player
    {
        public User() {
            setName("User-Human1");
        }

        public User(String name)
        {
            setName(name);
        }
        public override bool analizeField(char[,] field, char clearCell)
        {


            ConsoleKeyInfo infoKey = Console.ReadKey();
            //if ((infoKey.Modifiers & ConsoleModifiers.Control) != 0) Console.Write("CTL+");
            if (infoKey.Key == ConsoleKey.LeftArrow)
            {
                if (this.currentX > 0)
                {
                    --this.currentX;
                }
            }
            if (infoKey.Key == ConsoleKey.RightArrow)
            {
                if (this.currentX < field.GetLength(1) - 1)
                {
                    ++this.currentX;
                }
            }
            if (infoKey.Key == ConsoleKey.UpArrow)
            {
                if (this.currentY > 0)
                {
                    --this.currentY;
                }
            }
            if (infoKey.Key == ConsoleKey.DownArrow)
            {
                if (this.currentY < field.GetLength(0) - 1)
                {
                    ++this.currentY;
                }
            }

            if (infoKey.Key == ConsoleKey.Enter)
            {
                return false;
            }
            return true;

        }
    }
}
