using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleAppGameCrossZero
{
    public static class ConsoleHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool WriteConsoleOutputCharacter(IntPtr hConsoleOutput, string lpCharacter, uint nLength, Point16 dwWriteCoord, out uint lpNumberOfCharsWritten);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_INPUT_HANDLE = -10;
        private const int STD_ERROR_HANDLE = -12;
        private static readonly IntPtr _stdOut = GetStdHandle(STD_OUTPUT_HANDLE);

        private struct Point16
        {
            public short X;
            public short Y;

            public Point16(short x, short y)
                => (X, Y) = (x, y);
        };

        public static void WriteToBufferAt(string text, int x, int y)
        {
            WriteConsoleOutputCharacter(_stdOut, text, (uint)text.Length, new Point16((short)x, (short)y), out uint _);
        }
    }
}
