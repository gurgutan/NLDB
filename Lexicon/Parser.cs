using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NLDB
{
    public static class Parsers
    {

        private static string separators0 = @"";
        private static string separators1 = @"[^а-яА-ЯёЁ0-9]+"; //@"[\s\t\,\.\?\!\;\:\-\\\/]+";
        private static string separators2 = @"[\.\?\!\;\:]+";   //разделители простых предложений (в том числе и сложносочиненных)
        private static string separators3 = @"[\n\r]+";         //разделители абзацев
        private static string separators4 = @"\[\[\d+\]\]";     //разделители статей в wiki-file, напр. [[46577]]

        /// <summary>
        /// Парсер для создания терма (слова), элементами которого являются буквы
        /// </summary>
        public static Parser ParserRank0 = new Parser(separators0);
        /// <summary>
        /// Парсер для создания терма (предложения), элементами которого являются слова
        /// </summary>
        public static Parser ParserRank1 = new Parser(separators1);
        /// <summary>
        /// Парсер для создания терма (абзаца), элементами которого являются предложения
        /// </summary>
        public static Parser ParserRank2 = new Parser(separators2, ParserRank1);
        /// <summary>
        /// Парсер для создания терма (статьи), элементами которого являются абзацы
        /// </summary>
        public static Parser ParserRank3 = new Parser(separators3, ParserRank2);
        /// <summary>
        /// Парсер для создания терма (раздела), элементами которого являются статьи
        /// </summary>
        public static Parser ParserRank4 = new Parser(separators4, ParserRank3);    //статьи
    }

    public class Parser
    {
        public static string NullString = "";

        // Шаблон рег. выражения, содержащий правила разбиения текста на слова
        private string pattern = "";
        private Parser ChildParser = null;
        private Regex split_regex;
        private Regex remove_regex = new Regex(@"[a-z0-9\""\s\t\@\#\$]+", RegexOptions.Compiled);

        public Parser() { }

        public Parser(string ptrn)
        {
            pattern = ptrn;
            split_regex = new Regex(ptrn, RegexOptions.Compiled);
        }

        public Parser(string ptrn, Parser childParser)
        {
            pattern = ptrn;
            ChildParser = childParser;
            split_regex = new Regex(ptrn, RegexOptions.Compiled);
        }

        public Term TryParse(string line)
        {
            var symbols = this.Split(line);//this.GetMatches(line).Where(s => s.Length > 0);
            if (symbols.Count() == 0) return null;
            if (this.ChildParser == null)
                return new Term(Parser.NullString, symbols.Select(s => new Term(s)).ToArray());
            else
            {
                var childs = symbols.
                    Select(s => this.ChildParser.TryParse(s)).
                    Where(t => t != null);
                if (childs.Count() == 0) return null;
                return new Term(Parser.NullString, childs.ToArray());
            }
        }

        public IEnumerable<string> Split(string line)
        {
            line = FormatString(line);
            var s = split_regex.Split(line).Where(segment => segment.Length > 0);
            return s;
        }

        private string FormatString(string line)
        {
            return remove_regex.Replace(line, " ").ToLower();
        }

        private IEnumerable<string> GetMatches(string line)
        {
            MatchCollection matches = Regex.Matches(line, pattern, RegexOptions.Compiled);
            List<string> s = new List<string>();
            if (matches.Count == 0) return s;
            foreach (Match m in matches)
            {
                string tmp = m.Value.Trim().ToLower();
                if (String.IsNullOrEmpty(tmp)) continue;
                s.Add(tmp);
            }
            return s;
        }
    }
}
