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
            Assert.Fail();
        }

        [TestMethod()]
        public void GetWordTest()
        {
            Assert.Fail();
        }
    }
}