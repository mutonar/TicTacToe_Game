using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleAppGameCrossZero
{
    class Program
    {
        private int maxSize = 5;
        private int widthField = 3;
        private int heightField = 3;
        private char[,] field;
        private int idToPlayer = 0;
        private LinkedList<Player> arrPlayer = new LinkedList<Player>();
        private InterfaceDispalay interfaceDispalay;
        private char[] markUserCell = new char[] {'x', 'O', 'T'}; // для трех игроков
        private char markTmp = '.';
        private char clearCell = '*';
        private String InfoGame = "";

        public StatesGame stateGame = StatesGame.start;


        public enum StatesGame
        {
            start,
            run,
            end,
            restart
        }

        public Program(int x, int y) {
            setSizeField(y, x); // сразу генерим поле
            setOutPutInterface(new OutputDataToConsole()); // вывод сразу на консоль, можно изменить в момент выполнения
        }

        public StatesGame getStateGame()
        {
            return stateGame;
        }

        public void setStateGame(StatesGame stateGame)
        {
            this.stateGame = stateGame;
        }
        public void setOutPutInterface(InterfaceDispalay interfaceDispalay)
        {
            this.interfaceDispalay = interfaceDispalay;
        }

        public void addPlayer(Player player) {
            player.setId(idToPlayer); // при добавление с первого должен быть
            player.setMarkCell(markUserCell[idToPlayer]);
            arrPlayer.AddLast(player);
            idToPlayer++;
        }

        private Player checkFileldToVictory()
        {
            Player victoryPlayer = null;
            // проверка столбцов
            for (int i = 0; i < field.GetLength(1); i++)
            {
                int countColumnEquels = 0;
                for (int j = 0; j < field.GetLength(0) - 1; j++)
                {
                    char whoIdCell = field[j, i];
                    char nextCell = field[j + 1, i];
                    if (whoIdCell == nextCell & whoIdCell != clearCell)
                    {
                        ++countColumnEquels;
                    }
                    else {
                        countColumnEquels = 0;
                    }
                }
                if (countColumnEquels >= field.GetLength(0) - 1)
                {
                    foreach (Player item in arrPlayer)
                    {
                        if (item.getMarkCell() == field[0, i])
                        {
                            victoryPlayer = item;
                            break;
                        }
                    }
                    break;
                }
            }

            if (victoryPlayer != null) //пролетаем проверку столбцов
            {
                return victoryPlayer;
            }

            // проверка строк
            for (int i = 0; i < field.GetLength(0); i++)
            {
                int countRowEquels = 0;
                for (int j = 0; j < field.GetLength(1) - 1; j++)
                {
                    char whoIdCell = field[i, j];

                    if (whoIdCell == field[i, j + 1] & whoIdCell != clearCell)
                    {
                        ++countRowEquels;
                    }
                    else
                    {
                        countRowEquels = 0;
                    }
                }
                if (countRowEquels >= field.GetLength(1) - 1)
                {
                    foreach (Player item in arrPlayer)
                    {
                        if (item.getMarkCell() == field[i, 0])
                        {
                            victoryPlayer = item;
                            break;
                        }
                    }
                    break;
                }
            }

            // на занятые все клетки
            if (victoryPlayer == null)
            {
                int busyCell = 0;
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        if (field[i, j] != clearCell) {
                            ++busyCell;
                        }
                    }
                }
                if (busyCell >= field.GetLength(0) * field.GetLength(1))
                {
                    return new User("Draw !!!");
                }
            }
            return victoryPlayer;
        }
        private void setSizeField(int x, int y)
        {
            if (x <= maxSize & y <= maxSize & x > 0 & y > 0)
            {
                widthField = x;
                heightField = y;
            }
            field = new char[widthField, heightField];
            for (int i = 0; i < widthField; i++)
            {
                for (int j = 0; j < heightField; j++)
                {
                    field[i, j] = clearCell;
                }
            }
        }

        private void setInfoGame(String InfoGame)
        {
            this.InfoGame = InfoGame;
        }
        private String getInfoGame()
        {
            return InfoGame;
        }
        private void setStepPlayers() // ход каждого пользователя
        {
            foreach (Player item in arrPlayer)
            {
                String NamePlayer = item.getName();
                if (item.getOptionInfo() != null)
                {
                    string inf = item.getOptionInfo();
                    setInfoGame(inf);
                }
                interfaceDispalay.showFrame(field, new String[] { NamePlayer });
                int[] XYcellUser = new int[2];
                while (item.setStep(field, clearCell) || (field[XYcellUser[0], XYcellUser[1]] == clearCell) == false)  // Энтер и свободная клетка
                {
                    char[,] fieldTMP = new char[field.GetLength(0), field.GetLength(1)];
                    for (int i = 0; i < field.GetLength(0); i++)
                    {
                        for (int j = 0; j < field.GetLength(1); j++)
                        {
                            fieldTMP[i, j] = field[i, j];
                        }
                    }

                    String addetionInfo = "";
                    XYcellUser = item.getSelectCell();
                    fieldTMP[XYcellUser[0], XYcellUser[1]] = markTmp;
                    if ((field[XYcellUser[0], XYcellUser[1]] != clearCell))
                    {
                        addetionInfo += "Cell is busy!!!";
                    }
                    interfaceDispalay.showFrame(fieldTMP, new String[] { NamePlayer, addetionInfo , getInfoGame() });
                } 
                
                XYcellUser = item.getSelectCell(); // костыль
                interfaceDispalay.showFrame(field, new String[] { NamePlayer , getInfoGame() });
                field[XYcellUser[0], XYcellUser[1]] = item.getMarkCell();

                if (checkFileldToVictory() != null)
                {
                    String v = "VICTORY or Row !";
                    String key = "Press any key to exit or restart key-\"R\".";
                    interfaceDispalay.showFrame(field, new String[] { checkFileldToVictory().getName(),  v, key });
                    Thread.Sleep(1000);
                    ConsoleKeyInfo infoKey = Console.ReadKey();
                    if (infoKey.Key == ConsoleKey.E)
                    {
                        stateGame = StatesGame.end;
                        
                    }
                    else 
                    {
                        stateGame = StatesGame.restart;
                    }
                    break;
                }
            }
        }
        public 
        static void Main(string[] args)
        {
            int x = 3;
            int y = 3;
            Program program = new Program(x, y);

            while (program.getStateGame() != StatesGame.end)
            {
                program.setStepPlayers();
                switch (program.getStateGame())
                {
                    case StatesGame.start:
                        {
                            program.addPlayer(new Enemy2());
                            //program.addPlayer(new Enemy2("2 II"));
                            program.addPlayer(new User("Player1"));
                            //program.addPlayer(new User("Player2"));
                            program.setStateGame(StatesGame.run);
                            break;
                        }
                    case StatesGame.restart:
                        {
                            program = new Program(x, y);
                            program.setStateGame(StatesGame.start);
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }
}
