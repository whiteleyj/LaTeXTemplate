
namespace LaTeXTemplate.Tests
{
    using LaTeXTemplate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass()]
    public class TemplateReaderTest
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

        string _simpleTemplate = @"
This is the start of the document
%<: ReportTitle >%
this is the subtitle.

beginLongTable{}
longTableLine{ID & Name & Quantity & Amount}

%<for: ReportItem item >%
%<  /longTableLine: {item.Id} {item.Name} {item.Quantity} {item.Amount}>%
%<      if: item.HasSubItems>%
%<          for: item.SubItems subItem>%

longTableLine{Sub Items & & Name & Price}

%<              /longTableSubLine: {subItem.Name} {subItem.Price}>%
%<          end:for>%
%<      end:if>%
%<<end:for>%

Grand Totals:
%<:TotalItems>";

        [TestMethod()]
        public void GetTemplateChunks_GetsTheRightNumberOfChunks()
        {
            TemplateReader target = new TemplateReader();
            target.TemplateText = "Text %<:Data>% More Text";
            var chunks = target.GetTemplateChunks().ToList();
            Assert.IsTrue(chunks.Count == 3);
        }

        [TestMethod()]
        public void GetTemplateChunks_FirstChunkIsCorrect()
        {
            TemplateReader target = new TemplateReader();
            target.TemplateText = "Text %<:Data>% More Text";
            var chunks = target.GetTemplateChunks().ToList();
            Assert.IsTrue(chunks[0] == "Text ");
        }

        [TestMethod()]
        public void GetTemplateChunks_LastChunkIsCorrect()
        {
            TemplateReader target = new TemplateReader();
            target.TemplateText = "Text %<:Data>%   More Text  ";
            var chunks = target.GetTemplateChunks().ToList();
            Assert.IsTrue(chunks[2] == "   More Text  ");
        }

        [TestMethod()]
        public void GetTemplateChunks_DataChunkIsCorrect()
        {
            TemplateReader target = new TemplateReader();
            target.TemplateText = "Text %<:Data>%   More Text  ";
            var chunks = target.GetTemplateChunks().ToList();
            Assert.IsTrue(chunks[1] == "%<:Data>%");
        }

        [TestMethod()]
        public void GetTemplateChunks_DataChunkIsFirst()
        {
            TemplateReader target = new TemplateReader();
            target.TemplateText = "%<:Data>%text  ";
            var chunks = target.GetTemplateChunks().ToList();
            Assert.IsTrue(chunks.Count == 2);
            Assert.IsTrue(chunks[0] == "%<:Data>%");
            Assert.IsTrue(chunks[1] == "text  ");
        }
    }
}
