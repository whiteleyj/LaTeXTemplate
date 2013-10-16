namespace LaTeXTemplate.Tests
{
    using LaTeXTemplate;
    using LaTeXTemplate.Components;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Collections.Generic;


    [TestClass()]
    public class DocumentComponentTests
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
        public void Document_RenderTest()
        {
            TemplateBuilder target = new TemplateBuilder();
            IEnumerable<string> chunks = new List<string>() 
            {
                "%<for:Rows r>%",
                "%<r.Name>%", 
                "%<r.Amount>%" , 
                "%<r.Quantity>%",
                @"
",
                "%<end:for>%"
            };
            var doc = target.BuildTemplateDoc(chunks);
            var td = TestReportData.GetTestData();
            var dc = new DataChain(td);
            var output = doc.Render(dc);

            // concatnate all the data into a string.
            string testRender = td.Rows.Select(r => r.Amount.ToString() + r.Name.ToString() + r.Quantity.ToString())
                                       .Aggregate((x, y) => x + y);

            // the rendered output should be longer than the data it contains.
            Assert.IsTrue(output.Length > testRender.Length);
        }

        [TestMethod()]
        public void LaTeX_RenderTest()
        {
            LaTeXComponent cmd = new LaTeXComponent();
            cmd.CommandName = @"\totals";
            cmd.SetParamString("{Items}{Totals}");

            var testData = TestReportData.GetTestData();
            testData.Totals = 10;
            testData.Items = 3;

            DataChain obj = new DataChain(testData.GetType());
            obj.CurrentObj = testData;

            var output = cmd.Render(obj);
            Assert.AreEqual(@"\totals{3}{10}", output);
        }

        [TestMethod()]
        public void LaTeX_SetParamStringTest()
        {
            LaTeXComponent cmd = new LaTeXComponent();
            cmd.SetParamString("{Items}{Totals}");
            Assert.AreEqual(2, cmd.Params.Count);
            Assert.AreEqual("Items", cmd.Params[0]);
            Assert.AreEqual("Totals", cmd.Params[1]);
        }
    }
}
