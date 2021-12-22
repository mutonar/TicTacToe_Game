using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleAppGameCrossZero
{
    class Enemy : Player
    {
        public Enemy()
        {
            setName("II");
        }
        public override bool analizeField(char[,] field, char clearCell)
        {
            //Thread.Sleep(1000); // типа думаем
            int[] cellOnField = new int[2];
            
            int maxDangerRow = 0;
            int maxDangerColumn = 0;
            float maxFillRowEnemy = 0;
            float maxFillColumnEnemy = 0;
            
            int maxParitetRow = 0;
            int maxParitetColumn = 0;
            float maxFillRowMy = 0;
            float maxFillColumnMY = 0;

            // Суммы весов по осям когда не равны
            float summWeightX = 0;
            for (int x = 0; x < field.GetLength(1); x++)
            {
                summWeightX += 1 - (Math.Abs((float)x / (field.GetLength(1) - 1) * 2.0f - 1.0f));
            }
            float summWeightY = 0;
            for (int y = 0; y < field.GetLength(0); y++)
            {
                summWeightY += 1 - (Math.Abs((float)y / (field.GetLength(0) - 1) * 2.0f - 1.0f));
            }
            // проверка столбцов
            for (int i = 0; i < field.GetLength(1); i++)
            {
                float TmpProcentFillColumnRow = 0;
                float TmpProcentExistMyCell = 0;
                float currentSummColumn = 0;
                bool findCellUser = false;
                bool findCellMine = false;
                for (int j = 0; j < field.GetLength(0); j++)
                {
                    char dataCell = field[j, i];
                    // вес чисел в матрице
                    
                    float yWeightCell = (1 - (Math.Abs((float)j / (field.GetLength(0) - 1) * 2.0f - 1.0f))) * (1 / summWeightY);
                    float xWeightCell = (1 - (Math.Abs((float)i / (field.GetLength(1) - 1) * 2.0f - 1.0f))) * (1 / summWeightX);
                    float weightCell = yWeightCell + xWeightCell;
                    currentSummColumn += yWeightCell;

                    if (dataCell == clearCell) {
                        if (!findCellUser)
                        {
                            TmpProcentExistMyCell += weightCell;
                        }
                        if (!findCellMine)
                        {
                            TmpProcentFillColumnRow += weightCell;
                        }
                    }

                        // наша ли это клетка на этой строке
                    if (dataCell == getMarkCell())
                    {
                        findCellMine = true;
                        if (findCellUser)
                        {
                            TmpProcentExistMyCell = 0;
                            maxParitetColumn = 0;
                        }
                        else
                        {
                            TmpProcentExistMyCell += weightCell;
                        }
                    }

                    if ( // условие определение противника
                         dataCell != getMarkCell() &
                         dataCell != clearCell
                         )
                    {
                        findCellUser = true;
                        if (findCellMine)
                        {
                            TmpProcentFillColumnRow = 0;
                            maxDangerColumn = 0; // просто по умолчанию
                        }
                        else
                        {
                            TmpProcentFillColumnRow += weightCell;
                        }
                    }
                }

                if (TmpProcentFillColumnRow > maxFillColumnEnemy)
                {
                    maxFillColumnEnemy = TmpProcentFillColumnRow;
                    maxDangerColumn = i;
                }
                if (TmpProcentExistMyCell > maxFillColumnMY) // условие своих клеток
                {
                    maxFillColumnMY = TmpProcentExistMyCell;
                    maxParitetColumn = i;
                }
            }
            // проверка строк
            for (int i = 0; i < field.GetLength(0); i++)
            {
                float TmpProcentFillRow = 0;
                float TmpProcentexistMyCell = 0;
                bool findCellUser = false;
                bool findCellMine = false;
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    char dataCell = field[i, j];
                    // вес чисел в матрице
                    float weightCell = (1 - (Math.Abs((float)j / (field.GetLength(1) -1) * 2.0f - 1.0f))) * (1 / summWeightX);
                   // float yweightCell = (1 - (Math.Abs((float)i / (field.GetLength(0) - 1) * 2.0f - 1.0f))) * (1 / summWeightY);
                    //float weightCell = xweightCell + yweightCell ;

                    if (dataCell == clearCell)
                    {
                        if (!findCellUser)
                        {
                            TmpProcentexistMyCell += weightCell;
                        }
                        if (!findCellMine)
                        {
                            TmpProcentFillRow += weightCell;
                        }
                    }
                    // наша ли это клетка на этой строке
                    if (dataCell == getMarkCell())
                    {
                        findCellMine = true;
                        if (findCellUser)
                        {
                            TmpProcentexistMyCell = 0;
                            maxParitetRow = 0;
                        }
                        else
                        {
                            TmpProcentexistMyCell += weightCell;
                        }
                    }

                    if ( // условие определение противника
                        dataCell != getMarkCell() &
                        dataCell != clearCell
                        )
                    {
                        findCellUser = true;
                        if (findCellMine)
                        {
                            TmpProcentFillRow = 0;
                            maxDangerRow = 0; // просто по умолчанию
                        }
                        else
                        {
                            TmpProcentFillRow += weightCell;
                        }
                    }
                }
                if (TmpProcentFillRow > maxFillRowEnemy)
                {
                    maxFillRowEnemy = TmpProcentFillRow;
                    maxDangerRow = i;
                }
                if (TmpProcentFillRow > maxFillRowMy) // условие своих клеток
                {
                    maxFillRowMy = TmpProcentFillRow;
                    maxParitetRow = i;
                }
            }

            // анализ собранных данных по весам
            bool checkRow = true;
            int countRowOrColumn = 0; // куда ставить будем значение
            
            if (maxFillRowEnemy >= maxFillColumnEnemy) // если опасность строки более опасно чем столбца
            {
                if (maxFillRowMy >= maxFillColumnMY)
                {
                    if (maxFillRowEnemy == maxFillRowMy)
                    {
                        if (getId() <= 0) // Если первые ходим
                        {
                            countRowOrColumn = maxParitetRow;
                            checkRow = true;
                        }
                        else
                        {
                            countRowOrColumn = maxDangerRow;
                            checkRow = true;
                        }
                    }
                    else
                    {
                        if (maxFillRowEnemy > maxFillRowMy)
                        {
                            countRowOrColumn = maxDangerRow;
                            checkRow = true;
                        }
                        else
                        {
                            countRowOrColumn = maxParitetRow;
                            checkRow = true;
                        }
                    }
                }
                else
                {
                    if (maxFillRowEnemy == maxFillColumnMY)
                    {
                        if (getId() <= 0) // Если первые ходим
                        {
                            countRowOrColumn = maxParitetColumn;
                            checkRow = false;
                        }
                        else
                        {
                            countRowOrColumn = maxDangerRow;
                            checkRow = true;
                        }
                    }
                    else
                    {
                        if (maxFillRowEnemy > maxFillColumnMY)
                        {
                            countRowOrColumn = maxDangerRow;
                            checkRow = true;
                        }
                        else
                        {
                            countRowOrColumn = maxParitetColumn;
                            checkRow = false;
                        }
                    }
                }
            }
            else ////////////////////////////////////////
            {
                if (maxFillRowMy >= maxFillColumnMY)
                {
                    if (maxFillColumnEnemy == maxFillRowMy)
                    {
                        if (getId() <= 0) // Если первые ходим
                        {
                            countRowOrColumn = maxParitetRow;
                            checkRow = true;
                        }
                        else
                        {
                            countRowOrColumn = maxDangerColumn;
                            checkRow = false;
                        }
                    }
                    else
                    {
                        if (maxFillColumnEnemy > maxFillRowMy)
                        {
                            countRowOrColumn = maxDangerColumn;
                            checkRow = false;
                        }
                        else
                        {
                            countRowOrColumn = maxParitetRow;
                            checkRow = true;
                        }
                    }
                }
                else
                {
                    if (maxFillColumnEnemy == maxFillColumnMY)
                    {
                        if (getId() <= 0) // Если первые ходим
                        {
                            countRowOrColumn = maxParitetColumn;
                            checkRow = false;
                        }
                        else
                        {
                            countRowOrColumn = maxDangerColumn;
                            checkRow = false;
                        }
                    }
                    else
                    {
                        if (maxFillColumnEnemy > maxFillColumnMY)
                        {
                            countRowOrColumn = maxDangerColumn;
                            checkRow = false;
                        }
                        else
                        {
                            countRowOrColumn = maxParitetColumn;
                            checkRow = false;
                        }
                    }
                }
            }

            if (checkRow)
            {
                for (int i = 0; i < field.GetLength(1) - field.GetLength(1) / 2; i++)
                {
                    char dataCell = field[countRowOrColumn, maxParitetColumn]; 
                    if (dataCell == clearCell) // максимально выгодное
                    {
                        this.currentY = countRowOrColumn;
                        this.currentX = maxParitetColumn;
                        break;
                    }
                    if (field.GetLength(1) % 2 == 0)
                    {
                        int x = field.GetLength(1) / 2 - 1 + i;
                        dataCell = field[countRowOrColumn, x];
                        if (dataCell == clearCell)
                        {
                            this.currentY = countRowOrColumn;
                            this.currentX = x;
                            break;
                        }
                        x = field.GetLength(1) / 2 - i;
                        dataCell = field[countRowOrColumn, x];
                        if (dataCell == clearCell)
                        {
                            this.currentY = countRowOrColumn;
                            this.currentX = x;
                            break;
                        }
                    }
                    else
                    {
                        int x = field.GetLength(1) / 2 + i;
                        dataCell = field[countRowOrColumn, x];
                        if (dataCell == clearCell)
                        {
                            this.currentY = countRowOrColumn;
                            this.currentX = x;
                            break;
                        }
                        x = field.GetLength(1) / 2 - i;
                        dataCell = field[countRowOrColumn, x];
                        if (dataCell == clearCell)
                        {
                            this.currentY = countRowOrColumn;
                            this.currentX = x;
                            break;
                        }
                    }
                    
                }
            }
            else // ставим на столбцы
            {
                for (int i = 0; i < field.GetLength(0) - field.GetLength(0) /2 ; i++)
                {
                    //char dataCell = field[i, countRowOrColumn];
                    char dataCell = field[maxParitetRow, countRowOrColumn];
                    if (dataCell == clearCell) // максимально выгодное
                    {
                        this.currentY = maxParitetRow;
                        this.currentX = countRowOrColumn;
                        break;
                    }
                    /*if (dataCell == clearCell)
                    {
                        this.currentY = i;
                        this.currentX = countRowOrColumn;
                        break;
                    }
                    */
                    if (field.GetLength(0) % 2 == 0)
                    {
                        int y = field.GetLength(0) / 2 - 1 + i;
                        dataCell = field[y, countRowOrColumn];
                        if (dataCell == clearCell)
                        {
                            this.currentY = y;
                            this.currentX = countRowOrColumn;
                            break;
                        }
                        y = field.GetLength(0) / 2 - i;
                        dataCell = field[y, countRowOrColumn];
                        if (dataCell == clearCell)
                        {
                            this.currentY = y;
                            this.currentX = countRowOrColumn;
                            break;
                        }
                    }
                    else
                    {
                        int y = field.GetLength(0) / 2 + i;
                        dataCell = field[y, countRowOrColumn];
                        if (dataCell == clearCell)
                        {
                            this.currentY = y;
                            this.currentX = countRowOrColumn;
                            break;
                        }
                        y = field.GetLength(0) / 2 - i;
                        dataCell = field[y, countRowOrColumn];
                        if (dataCell == clearCell)
                        {
                            this.currentY = y;
                            this.currentX = countRowOrColumn;
                            break;
                        }
                    }
                }
            }
            return false;
        }
    }
}
