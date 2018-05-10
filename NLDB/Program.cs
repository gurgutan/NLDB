using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexicon;

namespace NLDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            const int size = 1024;
            SparseVector v1 = new SparseVector(new int[] { 1, 2, 3, 4, 5 });
            Console.WriteLine(v1.ToString());

            SparseMatrix m1 = new SparseMatrix(size, size);
            for (int i = 0; i < size; i++)
            {
                m1[rand.Next(size), rand.Next(size)] = rand.Next(2);
            }
            Console.WriteLine(m1.ToString());
            Console.ReadKey();
        }
    }
}
