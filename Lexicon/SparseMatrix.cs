using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexicon
{
    //Пара разреженных векторов: Item1 - строка, Item2 - колонка
    using SparseVectorPair = Tuple<SparseVector, SparseVector>;

    public class SparseMatrix
    {
        private int maxIndex;
        private int rowsCount;
        private int columnsCount;
        //Сортированный список пар векторов <строка, колонка>
        private SortedList<int, SparseVectorPair> data;

        public SparseMatrix()
        {
            rowsCount = 256;
            columnsCount = 256;
            maxIndex = 256;
            data = new SortedList<int, SparseVectorPair>(maxIndex);
            for (int i = 0; i < maxIndex; ++i)
                data[i] = new SparseVectorPair(new SparseVector(), new SparseVector());
        }

        public SparseMatrix(int n, int m)
        {
            rowsCount = n;
            columnsCount = m;
            maxIndex = this.Max(n, m);
            data = new SortedList<int, SparseVectorPair>(maxIndex);
            //for (int i = 0; i < maxIndex; ++i)
            //    data[i] = new SparseVectorPair(new SparseVector(), new SparseVector());
        }

        private SparseVectorPair GetVectorsPair(int i)
        {
            SparseVectorPair p;
            if (!data.TryGetValue(i, out p)) return null;
            return p;
        }

        private SparseVectorPair InitVectorsPair(int i)
        {
            SparseVectorPair p = new SparseVectorPair(new SparseVector(), new SparseVector());
            data[i] = p;
            return p;
        }

        private SparseVectorPair GetOrAddPair(int i)
        {
            SparseVectorPair p = GetVectorsPair(i);
            if (p == null) return InitVectorsPair(i);
            else return p;
        }

        /// <summary>
        /// Метод записывает значение val по адресу [i,j]
        /// </summary>
        /// <param name="i">номер строки (начинается с 0)</param>
        /// <param name="j">номер столбца (начинается с 0)</param>
        /// <param name="val">значение</param>
        public void SetValue(int i, int j, int val)
        {
            if (i >= maxIndex || j > maxIndex) maxIndex = Max(i, j);
            SparseVectorPair pairForRow = GetOrAddPair(i);
            SparseVectorPair pairForColumn = GetOrAddPair(j);
            pairForRow.Item1[j] = val;
            pairForColumn.Item2[i] = val;
        }

        public int GetValue(int i, int j)
        {
            SparseVectorPair p = GetVectorsPair(i);
            if (p==null) return 0;
            return p.Item1[j];
        }

        public int this[int i, int j]
        {
            get
            {
                return GetValue(i, j);
            }
            set
            {
                SetValue(i, j, value);
            }
        }

        public void SetColumn(int j, SparseVector v)
        {
            var indexes = v.AsIndexed();
            foreach (var i in indexes)
                this.SetValue(i.Item1, j, i.Item2);
        }

        public void ZeroColumn(int i)
        {
            SparseVectorPair pair = GetVectorsPair(i);
            if (pair == null) return;
            pair.Item1.Zeroize(i);  // обнуляем i-й элемент в строке
            pair.Item2.Zeroize();   // обнуляем всю i-ю колонку
        }
        public void SetRow(int i, SparseVector v)
        {
            var indexes = v.AsIndexed();
            foreach (var j in indexes)
                this.SetValue(i, j.Item1, j.Item2);
        }

        public void ZeroRow(int i)
        {
            SparseVectorPair pair = GetVectorsPair(i);
            if (pair == null) return;
            pair.Item1.Zeroize();   // обнуляем i-й элемент в колонке
            pair.Item2.Zeroize(i);  // обнуляем всю i-ю строку
        }


        //public Tuple<int, int>[] GetIndexedColumn(int i)
        //{
        //    return columns[i].Select(kvp => new Tuple<int, int>(kvp.Key, kvp.Value)).ToArray();
        //}

        //public Tuple<int, int>[] GetIndexedRow(int i)
        //{
        //    return rows[i].Select(kvp => new Tuple<int, int>(kvp.Key, kvp.Value)).ToArray();
        //}

        public SparseVector Row(int i)
        {
            SparseVectorPair pair = GetOrAddPair(i);
            return new SparseVector(pair.Item1);
        }

        public SparseVector Column(int i)
        {
            SparseVectorPair pair = GetOrAddPair(i);
            return new SparseVector(data[i].Item2);
        }

        public IEnumerable<Tuple<int, SparseVector>> Columns()
        {
            return data.
                Where(p1 => !p1.Value.Item2.IsZero()).
                Select(p => new Tuple<int, SparseVector>(p.Key, p.Value.Item2));
        }

        public IEnumerable<Tuple<int, SparseVector>> Rows()
        {
            return data.
                Where(p1 => !p1.Value.Item1.IsZero()).
                Select(p2 => new Tuple<int, SparseVector>(p2.Key, p2.Value.Item1));
        }

        public int FindEqualRow(SparseVector v)
        {
            KeyValuePair<int, SparseVectorPair> found =
                data.FirstOrDefault(d =>
                {
                    SparseVector dif = d.Value.Item1 - v;   //разница между строкой матрицы и вектором v
                    if (dif.SumMagnitude() == 0) return true;
                    else return false;
                });
            return found.Key;
        }

        public int FindEqualColumn(SparseVector v)
        {
            KeyValuePair<int, SparseVectorPair> found =
                data.AsParallel().
                FirstOrDefault(d =>
                {
                    SparseVector dif = d.Value.Item2 - v;   //разница между столбцом матрицы и вектором v
                    if (dif.SumMagnitude() == 0) return true;
                    else return false;
                });
            return found.Key;
        }

        public override string ToString()
        {
            return data.Aggregate("", (c, n) => c == "" ?
                n.ToString() :
                c + "\n" + n.ToString());
        }

        //Частные методы
        private int Min(int a, int b)
        {
            if (a < b) return a;
            else return b;
        }

        private int Max(int a, int b)
        {
            if (a > b) return a;
            else return b;
        }
    }
}
