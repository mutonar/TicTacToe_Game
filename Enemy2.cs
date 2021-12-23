using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleAppGameCrossZero
{
    class Enemy2 : Player
    {
        private int prioriteteRow = -1;
        private int prioriteteColumn = -1;
        private int prioriteRowEnemy = -1;
        private int prioriteColumnEnemy = -1;

        public Enemy2()
        {
            setName("Simple Enemy");
        }
        public Enemy2(String name)
        {
            setName(name);
        }
        public override string getOptionInfo()
        {
            return new string(getName() +":" +
            " prRow: " + prioriteteRow  +
            " prCol: " + prioriteteColumn +
            " prRowEn: " + prioriteRowEnemy +
            " prColEn: " + prioriteColumnEnemy
                );
        }
       
        public override bool analizeField(char[,] field, char clearCell)
        {
            
            char markEnemy = ']';
            char[,] fieldTmp = new char[field.GetLength(1), field.GetLength(0)];
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    fieldTmp[j, i] = field[i, j];
                    
                    char dataCell = field[i, j]; // за одно маркер врага ищем
                    if (dataCell != clearCell & dataCell != getMarkCell())
                    {
                        markEnemy = dataCell;
                    }
                }
            }

            prioriteteRow = checkePrioritetSetColumnRow(field, clearCell, getMarkCell());
            prioriteteColumn = checkePrioritetSetColumnRow(fieldTmp, clearCell, getMarkCell());
            prioriteRowEnemy = -1;
            prioriteColumnEnemy = -1;
            if (markEnemy != ']') {
                prioriteRowEnemy = checkePrioritetSetColumnRow(field, clearCell, markEnemy);
                prioriteColumnEnemy = checkePrioritetSetColumnRow(fieldTmp, clearCell, markEnemy);
            }

            int[] whereStep = analizeData(field, clearCell, getMarkCell(),
                prioriteteRow, prioriteteColumn, prioriteRowEnemy, prioriteColumnEnemy);

            if (whereStep[0] > -1 | whereStep[1] > -1)
            {
                this.currentX = whereStep[0];
                this.currentY = whereStep[1];
            }
            else
            {
                for (int i = 0; i < field.GetLength(0); i++) // такой расклад что не куда ходить больше по весам
                {

                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        char dataCell = field[i, j];
                        if (dataCell == clearCell)
                        {
                            this.currentY = i;
                            this.currentX = j;
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private int checkePrioritetSetColumnRow(char[,] field, char clearCell, char mark)
        {
            // проверка столбцов или строк в зависимости какой порядо прислали
            int prioritete = -1;
            float weght = 0;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                float weghtTMP = 0;
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    float yWeightCell = 1 - (Math.Abs((float)j / (field.GetLength(1) - 1) * 2.0f - 1.0f));
                    float xWeightCell = 1 - (Math.Abs((float)i / (field.GetLength(0) - 1) * 2.0f - 1.0f));


                    char dataCell = field[i, j];
                    if (dataCell == clearCell | dataCell == mark)
                    {
                        weghtTMP += yWeightCell + xWeightCell;
                    }
                    else
                    {

                        if ( // условие определение противника
                             dataCell != mark &
                             dataCell != clearCell
                             )
                        {
                            weghtTMP = 0;
                            break;
                        }
                    }
                }
                if (weghtTMP > weght)
                {
                    weght = weghtTMP;
                    prioritete = i;
                }
            }
            return prioritete;
        }

        private int[] analizeData(char[,] field, char clearCell, char mark,
            int prioriteteRow, int prioriteteColumn, int prioriteRowEnemy, int prioriteColumnEnemy)
        {
            float relationRow = (float)field.GetLength(0) / (float)field.GetLength(1);
            float relationColunm = (float)field.GetLength(1) / (float)field.GetLength(0);
            
            float sumColumnMy = 0;
            float sumColumnEnemy = 0;
            float sumRowMy = 0;
            float sumRowEnemy = 0;
            for (int i = 0; i < field.GetLength(0); i++) // приоритетные колонки
            {
                if (prioriteteColumn > -1)
                {
                    char dataCelMy = field[i, prioriteteColumn];
                    if (dataCelMy == mark)
                    {
                        ++sumColumnMy;
                    }
                }
                
                if (prioriteColumnEnemy > -1)
                {
                    char dataCelEnemy = field[i, prioriteColumnEnemy];
                    if (dataCelEnemy != clearCell & dataCelEnemy != mark)
                    {
                        ++sumColumnEnemy;
                    }
                }
            }
            for (int i = 0; i < field.GetLength(1); i++)
            {
                if (prioriteteRow > -1)
                {
                    char dataCelMy = field[prioriteteRow, i];
                    if (dataCelMy == mark)
                    {
                        ++sumRowMy;
                    }
                }
                if (prioriteRowEnemy > -1)
                {
                    char dataCelEnemy = field[prioriteRowEnemy, i];
                    if (dataCelEnemy != clearCell & dataCelEnemy != mark)
                    {
                        ++sumRowEnemy;
                    }
                }
            }
            sumColumnMy *= relationColunm;
            sumColumnEnemy *= relationColunm;
            sumRowMy *= relationRow;
            sumRowEnemy *= relationRow;

            int[] whereStep = new int[]{ -1, -1 };
            // первый ход
            if (sumColumnMy == 0 & sumColumnEnemy == 0 & 
            sumRowMy == 0 & sumRowEnemy == 0)
            {
                whereStep[0] = prioriteteColumn;
                whereStep[1] = prioriteteRow;
                return whereStep;
            }
            else // главная логика обработки приоритетных частей поля
            {
                if (sumColumnMy == sumColumnEnemy & sumRowMy == sumRowEnemy & // когда приоритеты равны
                    sumColumnMy == sumRowMy)
                {
                    if (sumColumnMy == sumColumnEnemy)
                    {
                        if (getId() <= 0) // Если первые ходим
                        {
                            if (field[prioriteteRow, prioriteteColumn] == clearCell)
                            {
                                whereStep[0] = prioriteteColumn;
                                whereStep[1] = prioriteteRow;
                            }
                            else
                            {
                                for (int i = 0; i < field.GetLength(0) - field.GetLength(0) / 2; i++) // если максимальное приемлемое не нашли ставим первое попавшее столбце(а надо из центра)
                                {
                                    // из центра сделал нужно в отдельный метод для диагностики каждого пункта.
                                    if (field.GetLength(0) % 2 == 0) // четная длинна массива
                                    {
                                        int count = field.GetLength(0) / 2 - 1 + i;
                                        if (field[count, prioriteteColumn] == clearCell)
                                        {
                                            whereStep[0] = prioriteteColumn;
                                            whereStep[1] = i;
                                        }
                                        else
                                        {
                                            count = field.GetLength(0) / 2 - i;
                                            if (field[count, prioriteteColumn] == clearCell)
                                            {
                                                whereStep[0] = prioriteteColumn;
                                                whereStep[1] = i;
                                            }
                                        }
                                        
                                    }
                                    else { // не четное
                                        int test = field.GetLength(0) / 2;
                                        int count = field.GetLength(0) / 2 + i;
                                        if (field[count, prioriteteColumn] == clearCell)
                                        {
                                            whereStep[0] = prioriteteColumn;
                                            whereStep[1] = i;
                                        }
                                        else
                                        {
                                            count = field.GetLength(0) / 2 - i;
                                            if (field[count, prioriteteColumn] == clearCell)
                                            {
                                                whereStep[0] = prioriteteColumn;
                                                whereStep[1] = i;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (field[prioriteRowEnemy, prioriteColumnEnemy] == clearCell)
                            {
                                whereStep[0] = prioriteColumnEnemy;
                                whereStep[1] = prioriteRowEnemy;
                            }
                            else
                            {
                                for (int i = 0; i < field.GetLength(0); i++)
                                {
                                    if (field[i, prioriteColumnEnemy] == clearCell)
                                    {
                                        whereStep[0] = prioriteColumnEnemy;
                                        whereStep[1] = i;
                                    }
                                }
                            }
                        }
                    }

                    if (sumRowMy == sumRowEnemy) // прверка приоритета строк
                    {
                        if (getId() <= 0) // Если первые ходим
                        {
                            if (field[prioriteteRow, prioriteteColumn] == clearCell)
                            {
                                whereStep[0] = prioriteteColumn;
                                whereStep[1] = prioriteteRow;
                            }
                            else
                            {
                                for (int i = 0; i < field.GetLength(1); i++) // если максимальное приемлемое не нашли ставим первое попавшее столбце(а надо из центра)
                                {
                                    if (field[prioriteteRow, i] == clearCell)
                                    {
                                        whereStep[0] = i;
                                        whereStep[1] = prioriteteRow;
                                    }

                                }
                            }
                        }
                        else
                        {
                            if (field[prioriteRowEnemy, prioriteColumnEnemy] == clearCell)
                            {
                                whereStep[0] = prioriteColumnEnemy;
                                whereStep[1] = prioriteRowEnemy;
                            }
                            else
                            {
                                for (int i = 0; i < field.GetLength(1); i++)
                                {
                                    if (field[prioriteRowEnemy, i] == clearCell)
                                    {
                                        whereStep[0] = i;
                                        whereStep[1] = prioriteRowEnemy;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    int x = 0;
                    int y = 0;
                    // Колонки больше 
                    if (sumColumnMy > sumColumnEnemy & sumColumnMy > sumRowMy & sumColumnMy > sumRowEnemy |
                        sumColumnMy < sumColumnEnemy & sumColumnEnemy > sumRowMy & sumColumnEnemy > sumRowEnemy                        
                        )
                    {
                        if (sumColumnMy > sumColumnEnemy)
                        {
                            x = prioriteteColumn;
                            y = prioriteteRow;
                        }
                        else
                        {
                            x = prioriteColumnEnemy;
                            y = prioriteRowEnemy;
                        }
                        {
                            if (x != -1 & y != -1 && field[y, x] == clearCell)
                            {
                                whereStep[0] = x;
                                whereStep[1] = y;
                            }
                            else
                            {
                                for (int i = 0; i < field.GetLength(0); i++) // если максимальное приемлемое не нашли ставим первое попавшее столбце(а надо из центра)
                                {
                                    if (field[i, x] == clearCell)
                                    {
                                        whereStep[0] = x;
                                        whereStep[1] = i;
                                    }

                                }
                            }
                        }

                    }
                    else
                    {
                        if (sumRowMy > sumRowEnemy) // тут не то
                        {
                            x = prioriteteColumn;
                            y = prioriteteRow;
                        }
                        else
                        {
                            x = prioriteColumnEnemy;
                            y = prioriteRowEnemy;
                        }

                        {
                            if (x != -1 & y != -1 && field[y, x] == clearCell)
                            {
                                whereStep[0] = x;
                                whereStep[1] = y;
                            }
                            else
                            {
                                for (int i = 0; i < field.GetLength(1); i++) // если максимальное приемлемое не нашли ставим первое попавшее столбце(а надо из центра)
                                {
                                    if (field[y, i] == clearCell)
                                    {
                                        whereStep[0] = i;
                                        whereStep[1] = y;
                                    }

                                }
                            }
                        }
                    }

                    

                }
            }

            

            return whereStep;
        }
    }
}
