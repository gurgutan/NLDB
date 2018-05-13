using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lexicon;

namespace NLDB
{
    class Program
    {
        static Lexicon ReadText(string filename)
        {
            Lexicon lex = new Lexicon();
            using (StreamReader reader = File.OpenText(filename))
            {
                int count = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Term term = Parsers.ParserRank2.TryParse(line);
                    if (term == null) continue;
                    int id = lex.Add(term);
                    count++;
                    Visualizer.WriteAtBeginOfLine("Обработано строк: " + count + " ", 800);
                }
            }
            return lex;
        }


        static void Main(string[] args)
        {

            Lexicon lex = ReadText("D:\\Data\\teachers.txt");
            Console.WriteLine();
            for (int i = 0; i < lex.RanksCount; i++)
            {
                var words = lex.GetWords(i);
                Console.WriteLine("Слов ранга {0}: {1}",i,words.Count());
            }
            
            Console.ReadKey();
        }
    }
}
