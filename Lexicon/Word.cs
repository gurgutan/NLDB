using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon
{
    public class Word
    {
        public int rank;
        public int id;
        public int[] childs;

        public Word() { }
        public Word(int r) { rank = r; }
        public Word(int _rank, int _id) { rank = _rank; id = _id; }
        public Word(int _rank, int _id, IEnumerable<int> _childs)
        {
            rank = _rank;
            id = _id;
            childs = new int[_childs.Count()];
            int i = 0;
            foreach (var c in _childs)
            {
                childs[i] = c;
                i++;
            }
        }
    }
}
