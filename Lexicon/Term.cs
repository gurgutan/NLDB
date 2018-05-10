using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLDB
{
    [Serializable]
    public class Term
    {
        public int Rank;
        public string Symbol;
        public Term[] Childs;

        public Term() 
        {
            Symbol = "-_-";
            Childs = new Term[0];
            Rank = 0;
        }

        public Term(string s)
        {
            if (String.IsNullOrEmpty(s)) throw new ArgumentNullException("Строка терма не может быть пустой");
            Symbol = s;
            Childs = new Term[0];
            Rank = 0;
        }

        public Term(string s, Term[] childs)
        {
            Symbol = s;
            Childs = childs;
            Rank = Childs[0].Rank + 1;
        }

        public override string ToString()
        {
            if (Childs.Length == 0) return Symbol.ToString();
            return "{" + Childs.Aggregate("", (c, n) => c == "" ? n.ToString() : c + "," + n.ToString()) + "}";
        }

    }
}
