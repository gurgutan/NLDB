using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLDB.Tests
{
    [TestClass()]
    public class LexiconTests
    {
        [TestMethod()]
        public void LexiconTest()
        {
            Lexicon lex = new Lexicon();
            Assert.AreEqual(lex.AsMatrices().Length, 4);
        }

        [TestMethod()]
        public void LinkTest()
        {
            Lexicon lex = new Lexicon();
            int rank = 0;
            int order = 7;
            int parent = 100;
            int child = 50;
            lex.Link(rank, parent, child, order);
            Assert.AreEqual(lex.Child(rank, parent, order), child);
        }

        [TestMethod()]
        public void ParentsTest()
        {
            Lexicon lex = new Lexicon();
            int rank = 0;
            int order = 7;
            int parent = 100;
            int child = 50;
            lex.Link(rank, parent, child, order);
            Assert.AreEqual(lex.Parents(rank, child).First(), parent);
        }

        [TestMethod()]
        public void ChildTest()
        {
            Lexicon lex = new Lexicon();
            int rank = 0;
            int order = 7;
            int parent = 100;
            int child = 50;
            lex.Link(rank, parent, child, order);
            Assert.AreEqual(lex.Child(rank, parent, order), child);
        }

        [TestMethod()]
        public void ChildsTest()
        {
            Lexicon lex = new Lexicon();
            int rank = 0;
            int order = 7;
            int parent = 100;
            int child = 50;
            lex.Link(rank, parent, child, order);
            int c = lex.Childs(rank, parent).First();
            Assert.AreEqual(c, child);
        }

        [TestMethod()]
        public void AddTest()
        {
            Lexicon lex = new Lexicon();
            string s = "привет";
            Term term = Parsers.ParserRank1.TryParse(s);
            int id = lex.Add(term);
            var words = lex.GetWords(1);
            Assert.AreEqual(words.Count(), 1);
        }

        [TestMethod()]
        public void GetWordTest()
        {
            Lexicon lex = new Lexicon();
            string s = "привет друзья. я пришел к вам в гости.";
            Term term = Parsers.ParserRank2.TryParse(s);
            int id = lex.Add(term);
            Assert.AreEqual(lex.GetWords(0).Count(), 8);
            Assert.AreEqual(lex.GetWords(1).Count(), 2);
            Assert.AreEqual(lex.GetWords(2).Count(), 1);
        }
    }
}