using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLDB
{
    public static class Visualizer
    {
        private static long counter = 0;

        public static void Clear()
        {
            Console.Clear();
        }

        public static void WriteAtBeginOfLine(string s)
        {
            Console.CursorLeft = 0;
            Console.Write(s);
        }

        public static void WriteAtBeginOfLine(string s, int lagtime)
        {
            long ticks = DateTime.Now.Ticks;
            if (ticks > counter)
            {
                counter = ticks + lagtime * 10000;
                Console.CursorLeft = 0;
                Console.Write(s);
            }
        }

        public static void WriteAtSamePosition(string s, int lagtime = 1000)
        {
            long ticks = DateTime.Now.Ticks;
            int pos_x = Console.CursorLeft;
            int pos_y = Console.CursorTop;
            Console.CursorVisible = false;
            if (ticks > counter)
            {
                counter = ticks + lagtime * 10000;
                Console.Write(s);
                Console.CursorLeft = pos_x;
                Console.CursorTop = pos_y;
            }
        }

        public static void ClearCurrentLine()
        {
            string l = " ".PadRight(Console.WindowWidth - 1);
            Visualizer.WriteAtBeginOfLine(l);
            Console.CursorLeft = 0;
        }

        public static void WriteWithPing(string s, int lagtime)
        {
            long ticks = DateTime.Now.Ticks;
            if (ticks > counter)
            {
                counter = ticks + lagtime * 10000;
                Console.Write(s);
            }
        }
    }
}
