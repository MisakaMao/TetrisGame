using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisGame_VS2008
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 30;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;

            GameProcess gameProcess = new GameProcess();
            gameProcess.StartGame();
        }
    }
}
