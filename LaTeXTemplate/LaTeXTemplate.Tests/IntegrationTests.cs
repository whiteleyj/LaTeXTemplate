namespace LaTeXTemplate.Tests.Integration
{
    using LaTeXTemplate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass()]
    [Ignore]
    public class IntegrationTests
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

        //[TestMethod()]
        [Ignore]
        public void IntegrationTest()
        {
            var reader = new TemplateReader();
            reader.TemplateText = @"
This is the start of the document
%<: Title >%
this is the subtitle.
beginLongTable{}
\longTableLine{ID & Name & Quantity & Amount}
%<for: Rows r >%
%<  \longTableLine: {r.Name} {r.Quantity} {r.Amount}>%
%<      if: r.HasSubItems>%
\longTableLine{Sub Items & & Name & Price}
%<          for: r.SubItems subItem>%
%<              \longTableSubLine: {subItem.Name} {subItem.Price}>%
%<          end:for>%
%<      end:if>%
%<end:for>%
Grand Totals:
%<Items>% - %<Total>%";

            var builder = new TemplateBuilder();
            var template = builder.BuildTemplateDoc(reader.GetTemplateChunks());
            var testData = TestReportData.GetTestData();

            var output = template.Render(testData);

            Assert.IsTrue(output.Length > reader.TemplateText.Length);
        }
    }
}
