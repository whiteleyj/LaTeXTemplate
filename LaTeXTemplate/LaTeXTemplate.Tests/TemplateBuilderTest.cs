
namespace LaTeXTemplate.Tests
{
    using LaTeXTemplate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using LaTeXTemplate.Components;

    [TestClass()]
    public class TemplateBuilderTest
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
        public void MakeComponents_MakesTextComponent()
        {
            TemplateBuilder target = new TemplateBuilder();
            IEnumerable<string> chunks = new List<string>() 
            {
                "Template!"     
            };
            var doc = target.BuildTemplateDoc(chunks);

            Assert.IsTrue(doc.SubComponent.Count == 1);
            Assert.IsTrue(doc.SubComponent[0] is TextComponent);
            Assert.AreEqual("Template!", doc.SubComponent[0].TemplateText);
        }

        [TestMethod()]
        public void MakeComponents_MakesVarComponent()
        {
            TemplateBuilder target = new TemplateBuilder();
            IEnumerable<string> chunks = new List<string>() 
            {
                "%<param>%"
            };
            var doc = target.BuildTemplateDoc(chunks);

            Assert.IsTrue(doc.SubComponent.Count == 1);
            Assert.IsTrue(doc.SubComponent[0] is VarComponent);
        }

        [TestMethod()]
        public void MakeComponents_MakesForComponent()
        {
            TemplateBuilder target = new TemplateBuilder();
            IEnumerable<string> chunks = new List<string>() 
            {
                "%<for:ReportItem item>%",
                "repeating text",
                "%<end:for>%"
            };
            var doc = target.BuildTemplateDoc(chunks);

            Assert.IsTrue(doc.SubComponent.Count == 1);
            Assert.IsTrue(doc.SubComponent[0] is ForComponent);
            Assert.IsTrue(doc.SubComponent[0].SubComponent.Count == 1);
            Assert.IsTrue(doc.SubComponent[0].SubComponent[0] is TextComponent);
        }

        // Makes latex command component

        [TestMethod()]
        public void MakeComponents_MakesLaTeXComponent()
        {
            TemplateBuilder target = new TemplateBuilder();
            IEnumerable<string> chunks = new List<string>() 
            {
                @"%<\latexFunction:{param1}{param2}{param3}>%"
            };

            var doc = target.BuildTemplateDoc(chunks);

            Assert.IsTrue(doc.SubComponent.Count == 1);
            Assert.IsTrue(doc.SubComponent[0] is LaTeXComponent);
        }
        // Makes if component


        [TestMethod()]
        public void BuildVarComponent_Simple()
        {
            TemplateBuilder target = new TemplateBuilder();
            var simp = target.BuildVarComponent(new LeafNode("%<PropertyName>%"));
            Assert.IsTrue(simp.ValueName == "PropertyName");
        }

        [TestMethod()]
        public void BuildVarComponent_Complex()
        {
            TemplateBuilder target = new TemplateBuilder();
            var comp = target.BuildVarComponent(new LeafNode("%<Something.PropertyName>%"));
            Assert.IsTrue(comp.ValueName == "PropertyName");
        }
    }
}
