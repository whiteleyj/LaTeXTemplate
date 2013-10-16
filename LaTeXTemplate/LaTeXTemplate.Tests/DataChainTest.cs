namespace LaTeXTemplate.Tests
{
    using LaTeXTemplate;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass()]
    public class DataChainTest
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
        public void GetValue_ReturnsTheValue_Test()
        {
            var testData = TestReportData.GetTestData();
            DataChain chain = new DataChain(testData.GetType());
            chain.CurrentObj = testData;
            string str = chain.GetValue("Title");
            Assert.AreEqual(testData.Title, str);
        }

        [TestMethod()]
        public void GetValue_GetDotSpecifiedName_Test()
        {
            var testData = TestReportData.GetTestData();
            DataChain chain = new DataChain(typeof(TestReportRow));
            chain.CurrentValName = "r";
            chain.CurrentObj = testData.Rows[0];
            string str = chain.GetValue("r.Name");
            Assert.AreEqual(testData.Rows[0].Name, str);
        }

        [TestMethod()]
        public void GetValue_BadValue_TheNameOfTheValueGetsReportedBack()
        {
            var testData = TestReportData.GetTestData();
            DataChain chain = new DataChain(testData.GetType());
            chain.CurrentObj = testData;
            Exception ex = null;

            try
            {
                string str = chain.GetValue("NotHere");
            }
            catch (Exception exept)
            {
                ex = exept;
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("NotHere"));
            Assert.IsTrue(ex.Message.Contains("Property"));
        }

        [TestMethod()]
        public void GetCollection_ReturnsTheCollection_Test()
        {
            var testData = TestReportData.GetTestData();
            DataChain chain = new DataChain(testData.GetType());
            chain.CurrentObj = testData;
            var returnedCollection = chain.GetCollection("Rows");
            Assert.AreEqual(testData.Rows, returnedCollection);
        }

        [TestMethod()]
        public void GetCollection_BadCollectionName_ThrowsAndReportsBadName()
        {
            var testData = TestReportData.GetTestData();
            DataChain chain = new DataChain(testData.GetType());
            chain.CurrentObj = testData;

            Exception ex = null;
            try
            {
                var returnedCollection = chain.GetCollection("NotRows");
            }
            catch (Exception exept)
            {
                ex = exept;
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex.Message.Contains("NotRows"));
            Assert.IsTrue(ex.Message.Contains("Collection"));
        }


        [TestMethod()]
        public void GetCollectionType_Test()
        {
            var td = TestReportData.GetTestData();
            DataChain chain = new DataChain(td);

            var colType = chain.GetCollectionsType("Rows");

            Assert.AreEqual(colType, typeof(TestReportRow));
        }

        [TestMethod()]
        public void GetValue_DemtersFolly()
        {
            var td = TestReportData.GetTestData();
            DataChain chain = new DataChain(typeof(TestReportRow));
            chain.CurrentValName = "r";
            chain.CurrentObj = td.Rows[0];

            Assert.AreEqual(chain.GetValue("Name"), td.Rows[0].Name);
            Assert.AreEqual(chain.GetValue("r.Name"), td.Rows[0].Name);
            Assert.AreEqual(chain.GetValue("foo.r.Name"), td.Rows[0].Name);
            Assert.AreEqual(chain.GetValue("foo.bar.baz.r.Name"), td.Rows[0].Name);  // watch him spin!
        }

        [TestMethod()]
        public void CurrentObj_ThrowsErrorWhenYouTryToAssignTheWrongThing()
        {
            string msg = "";
            try
            {
                var td = TestReportData.GetTestData();
                DataChain chain = new DataChain(typeof(TestReportRow));
                chain.CurrentObj = td;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }
            Assert.IsTrue(msg.Contains("TestReportData"));
            Assert.IsTrue(msg.Contains("TestReportRow"));
        }
    }
}