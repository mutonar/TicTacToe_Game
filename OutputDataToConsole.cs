using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppGameCrossZero
{
    class OutputDataToConsole : InterfaceDispalay
    {
        public void showFrame(char[,] dataDislay, String[] addInfoGame)
        {
            String strConsole = new String("╔" + new string('═', Console.WindowWidth - 2) + "╗");
            ConsoleHelper.WriteToBufferAt(strConsole, 0, 0); //это наша хитрая система
            int y = 0;
            int rowInfo = 0;
            for (int i = 1; i < Console.WindowHeight - 1; i++)
            {
                if (i > 2 & rowInfo < addInfoGame.Length)
                {
                    String rowInfoGame = addInfoGame[rowInfo];
                    strConsole = new string("║");
                    strConsole += new string(' ', Console.WindowWidth / 2 - rowInfoGame.Length/2);
                    strConsole += rowInfoGame;
                    strConsole += new string(' ', Console.WindowWidth / 2 - (rowInfoGame.Length - rowInfoGame.Length / 2) - 2); 
                    strConsole += "║";
                    ConsoleHelper.WriteToBufferAt(strConsole, 0, i); //это наша хитрая система
                    ++rowInfo;
                    continue;
                }
                int test1 = i - (Console.WindowHeight / 2 - dataDislay.GetLength(0) / 2);
                int test2 = i - (Console.WindowHeight / 2 + (dataDislay.GetLength(0) - dataDislay.GetLength(0) / 2));
                if ( // так данные должны быть по середине
                  i - (Console.WindowHeight / 2 - dataDislay.GetLength(0) / 2) >= 0 &&
                  test2  < 0
                  )
                {
                    strConsole = new string("║");
                    strConsole += new string(' ', Console.WindowWidth / 2 - dataDislay.GetLength(1) / 2);

                    for (int x = 0; x < dataDislay.GetLength(1); x++)
                    {
                        strConsole += dataDislay[y, x];
                    }

                    strConsole += new string(' ', Console.WindowWidth / 2 - (dataDislay.GetLength(1) - dataDislay.GetLength(1) / 2 ) - 2); // почему я подогнал под -2 ?
                    strConsole += "║";
                    strConsole += y.ToString();
                    ++y;
                }
                else
                {
                    strConsole = new String("║" + new string(' ', Console.WindowWidth - 2) + "║");
                    
                }
                //Console.Write(strConsole);
                ConsoleHelper.WriteToBufferAt(strConsole, 0, i); //это наша хитрая система
            }

            strConsole = new String("╚" + new string('═', Console.WindowWidth - 2) + "╝");
            ConsoleHelper.WriteToBufferAt(strConsole, 0, Console.WindowHeight - 1); //это наша хитрая система
            //Console.ReadKey(true);
        }
    }
}
