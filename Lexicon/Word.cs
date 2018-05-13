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
        public Word(int _rank, int _id) { rank = _rank; id = _id; childs = new int[0]; }
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

        public override string ToString()
        {
            return "[" +
                rank + "," +
                id + "," +
                (childs.Length == 0 ? "" :
                "{" + childs.Aggregate("", (c, n) => c == "" ? n.ToString() : c + "," + n.ToString()) + "}")
                + "]";
        }

        public string AsText(Dictionary<int, string> atoms)
        {
            return rank == 0 ? atoms[id] : "{" + childs.Aggregate("", (c, n) => c == "" ? n.ToString() : c + "," + n.ToString()) + "}";
        }
    }
}
