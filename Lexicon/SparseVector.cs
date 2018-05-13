using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon
{

    public class SparseVector
    {
        private SortedList<int, int> data;

        public SparseVector()
        {
            data = new SortedList<int, int>();
        }

        public SparseVector(int n)
        {
            data = new SortedList<int, int>(n);
        }

        public SparseVector(IEnumerable<int> values)
        {
            data = new SortedList<int, int>(values.Count());
            int i = 0;
            foreach (var v in values)
                data[i++] = v;
        }
        public SparseVector(IEnumerable<Tuple<int, int>> values)
        {
            data = new SortedList<int, int>(values.Count());
            foreach (var v in values)
                data[v.Item1] = v.Item2;
        }

        public SparseVector(IEnumerable<KeyValuePair<int, int>> values)
        {
            data = new SortedList<int, int>(values.Count());
            foreach (var v in values)
                data[v.Key] = v.Value;
        }

        public SparseVector(SparseVector other)
        {
            data = new SortedList<int, int>(other.data);
        }

        public int this[int i]
        {
            get
            {
                int val;
                if (data.TryGetValue(i, out val))
                    return val;
                else
                    return 0;
            }
            set
            {
                if (value == 0)
                {
                    if (data.ContainsKey(value))
                        data.Remove(i);
                }
                else
                    data[i] = value;
            }
        }

        public bool IsZero()
        {
            return data.Count() == 0;
        }

        public void Zeroize(int i)
        {
            if (data.ContainsKey(i))
                data.Remove(i);
        }

        public void Zeroize()
        {
            data.Clear();
        }

        public Tuple<int, int>[] AsIndexed()
        {
            return data.Select(kvp => new Tuple<int, int>(kvp.Key, kvp.Value)).ToArray();
        }

        /// <summary>
        /// Возвращает упорядоченную коллекцию индексов ненулевых элементов матрицы
        /// </summary>
        /// <returns>упорядоченная по возрастанию коллекция ненулевых индексов вектора</returns>
        public IEnumerable<int> Indexes()
        {
            return data.Keys.OrderBy(k => k);
        }

        public bool ContainIndex(int i)
        {
            return data.ContainsKey(i);
        }

        public static SparseVector operator -(SparseVector a, SparseVector b)
        {
            SparseVector result = new SparseVector();
            foreach (var v in b.AsIndexed())
                result[v.Item1] = a[v.Item1] - v.Item2;
            return result;
        }

        public static SparseVector operator +(SparseVector a, SparseVector b)
        {
            SparseVector result = new SparseVector();
            foreach (var v in b.AsIndexed())
                result[v.Item1] = a[v.Item1] + v.Item2;
            return result;
        }

        public static SparseVector operator *(SparseVector a, SparseVector b)
        {
            SparseVector result = new SparseVector();
            foreach (var v in b.AsIndexed())
                result[v.Item1] = a[v.Item1] * v.Item2;
            return result;
        }

        public int SumMagnitude()
        {
            return data.Sum(v => Math.Abs(v.Value));
        }

        public int Sum()
        {
            return data.Sum(v => v.Value);
        }

        public override string ToString()
        {
            return "{" + data.Aggregate("", (c, n) =>
            c == "" ?
            "[" + n.Key + "," + n.Value + "]" :
            c + "," + "[" + n.Key + "," + n.Value + "]") + "}";
        }

    }
}
