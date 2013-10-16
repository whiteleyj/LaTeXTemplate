namespace LaTeXTemplate.Tests
{
    using LaTeXTemplate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass()]
    public class LeafNodeTest
    {
        #region Additional test attributes
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        #endregion

        [TestMethod()]
        public void LeafNodeConstructorTest()
        {
            LeafNode target = new LeafNode("");
            Assert.IsTrue(target.Name == "");
            Assert.IsTrue(target.Params.Count == 0);
            Assert.IsFalse(target.IsNestedNode);
        }

        [TestMethod()]
        public void LeafNode_VarNode()
        {
            LeafNode target = new LeafNode("%< ReportTitle >%");
            Assert.AreEqual("ReportTitle", target.Name);
            Assert.AreEqual(0, target.Params.Count);
            Assert.IsFalse(target.IsNestedNode);
        }

        [TestMethod()]
        public void LeafNode_FuncNode()
        {
            LeafNode target = new LeafNode(@"%<  /longTableLine: {item.Id} {item.Name} {item.Quantity} {item.Amount}>%");
            Assert.AreEqual(@"/longTableLine", target.Name);
            Assert.AreEqual(4, target.Params.Count);
            Assert.AreEqual("{item.Id}", target.Params[0]);
            Assert.AreEqual("{item.Name}", target.Params[1]);
            Assert.AreEqual("{item.Quantity}", target.Params[2]);
            Assert.AreEqual("{item.Amount}", target.Params[3]);
            Assert.IsFalse(target.IsNestedNode);
        }

        [TestMethod()]
        public void LeafNode_ForNode()
        {
            LeafNode target = new LeafNode("%<for: ReportItem item >%");
            Assert.AreEqual("for", target.Name);
            Assert.AreEqual(2, target.Params.Count);
            Assert.AreEqual("ReportItem", target.Params[0]);
            Assert.AreEqual("item", target.Params[1]);
            Assert.IsTrue(target.IsNestedNode);
        }

        [TestMethod()]
        public void LeafNode_IfNode()
        {
            LeafNode target = new LeafNode("%<if: item.HasSubItems>%");
            Assert.AreEqual("if", target.Name);
            Assert.AreEqual(1, target.Params.Count);
            Assert.AreEqual("item.HasSubItems", target.Params[0]);
            Assert.IsTrue(target.IsNestedNode);
        }

        [TestMethod()]
        public void LeafNode_EndNode()
        {
            LeafNode target = new LeafNode("%<end: if >%");
            Assert.AreEqual("end", target.Name);
            Assert.AreEqual(1, target.Params.Count);
            Assert.AreEqual("if", target.Params[0]);
            Assert.IsFalse(target.IsNestedNode);
        }

        [TestMethod()]
        public void StripTemplateIndicatorsTest()
        {
            LeafNode target = new LeafNode("Bah");
            string actual = target.StripTemplateIndicators("%< blah blah blah     >%");
            Assert.AreEqual("blah blah blah", actual);
        }
    }
}