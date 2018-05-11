using Lexicon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NLDB
{
    public class Lexicon
    {
        private static readonly int MAX_ATOMS_N = 1 << 20;
        private static readonly int MAX_RANK = 4;
        private static readonly int MAX_N = 1048583;    // модуль класса вычетов, примитивные корни: {5,7,10,14,15,17,19,20,21,28,30,31,34,38,40,42,45,46,51,53,55,56,57,59,60,61,62,63,65,67,...}

        private Dictionary<string, int> atoms = new Dictionary<string, int>(MAX_ATOMS_N);
        private static int currentID = 2;
        //private int size;

        /// <summary>
        /// Матрицы композиций
        /// </summary>
        private SparseMatrix[] ranks = new SparseMatrix[MAX_RANK];

        public Lexicon()
        {
            for (int i = 0; i < MAX_RANK; i++)
                ranks[i] = new SparseMatrix(MAX_N, MAX_N);
        }

        public SparseMatrix[] AsMatrices()
        {
            return ranks;
        }

        public void Link(int r, int p, int c, int o)
        {
            ranks[r][c, p] = o;
        }

        public IEnumerable<int> Parents(int r, int c)
        {
            return ranks[r].Row(c).Indexes();
        }

        public int Child(int r, int p, int o)
        {
            return ranks[r].                        //среди слов ранга r
                Column(p).                          //в колонке дочерних элементов p
                AsIndexed().                        //среди пар <индекс, значение>
                FirstOrDefault(t => t.Item2 == o).  //первый, со значением o
                Item1;                              //индекс элемента
        }

        public IEnumerable<int> Childs(int r, int p)
        {
            return ranks[r].                //среди слов ранга r
                 Column(p).                 //в колонке дочерних элементов p
                 AsIndexed().               //пары <индекс, значение>
                 OrderBy(t => t.Item2).     //сортируем по значению
                 Select(pair => pair.Item1);//возвращаем только индексы
        }

        public int RanksCount
        {
            get
            {
                return ranks.Length;
            }
        }

        public int Add(Term term)
        {
            if (term.Rank == 0)
            {
                if (atoms.ContainsKey(term.Symbol)) return atoms[term.Symbol];
                int id = this.NextID();
                return atoms[term.Symbol] = id;
            }
            // Если слово ранга >0, то пытаемся его найти по дочерним словам
            SparseVector _childs = new SparseVector();
            for (int i=0; i<term.Childs.Length;i++)
            {
                _childs[this.Add(term.Childs[i])] = i;
            }
            int found = ranks[term.Rank - 1].FindEqualColumn(_childs);
            if (found > 0) return found;
            //Создаем новое слово
            int _id = this.NextID();
            ranks[term.Rank - 1].SetColumn(_id, _childs);
            return _id;
        }

        public Word GetWord(int r, int i)
        {
            // индекс матрицы равен рангу слова - 1
            SparseVector _childs = ranks[r - 1].Column(i);
            if (_childs.IsZero()) return null;
            return new Word(r, i, _childs.Indexes().OrderBy(v => v));
        }

        private int NextID()
        {
            currentID = (currentID * 7) % MAX_N;
            return currentID;
        }
    }
}
