using System;
using System.Collections.Generic;
using System.Text;
using static ConsoleAppGameCrossZero.Program;

namespace ConsoleAppGameCrossZero 
{
    abstract class Player : InterfaceControl
    {

        private String name = "";
        private int id = 0;
        private char markCell;
        protected int currentX = 0;
        protected int currentY = 0;
        protected char clearCell;
        protected StatePlayer state = StatePlayer.wait;

        public enum StatePlayer
        {
            think,
            surrender, // кома не может сдаться ведь
            wait,
            makedMove
        }

        private StatePlayer getStatusPlayer()// кострукция может быть и set get в одном
        {
            return state;
        }
        public bool setStep(char[,] field, char clearCell) // пока не true ход не сделан
        {
            this.clearCell = clearCell;
            return analizeField(field, clearCell); // пока не сделает шаг постоянная проверка
        }
        public int[] getSelectCell()
        {
            return new int[] { currentY, currentX };
        }

        public String getName()
        {
            return name;
        }
        public void setName(String name)
        {
            this.name = name;
        }

        internal void setId(int id)
        {
            this.id = id;
        }

        public int getId()
        {
            return id;
        }

        public void setMarkCell(char markCell)
        {
            this.markCell = markCell;
        }

        public char getMarkCell()
        {
            return markCell;
        }

        public virtual string getOptionInfo() {
            return null;
        }

        public abstract bool analizeField(char[,] field, char clearCell);


    }
}
