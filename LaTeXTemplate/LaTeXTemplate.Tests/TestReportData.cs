namespace LaTeXTemplate.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>
    /// 
    /// Holds test data for the other unit tests.
    /// 
    /// </summary>
    public class TestReportData
    {
        public string Title { get; set; }
        public decimal Totals { get; set; }
        public int Items { get; set; }
        public List<TestReportRow> Rows { get; set; }
        public decimal Total { get { return Rows.Select(r => r.Amount).Aggregate((x, y) => x + y); } }

        public static TestReportData GetTestData()
        {
            var d = new TestReportData();
            d.Title = "Test Report";
            d.Items = 10;
            d.Totals = 42.10m;
            d.Rows = new List<TestReportRow>()
            {
                new TestReportRow("Bill Nye", 4, 19.98m) 
                {
                    SubItems = new List<TestSubRow>() 
                    {
                        new TestSubRow(){ Name = "The Science Guy", Price = 10000.00m},
                        new TestSubRow(){ Name = "Weirdo", Price = 10.00m},
                        new TestSubRow(){ Name = "Bill Bill Bill Bill", Price = 92m}
                    }
                },
                new TestReportRow("Dave McCormic", 4, 19.98m),
                new TestReportRow("Randy Jackson", 4, 19.98m)
                {
                    SubItems = new List<TestSubRow>()
                    {
                        new TestSubRow() { Name = "The nice one.", Price = 0m}
                    }
                }
            };
            return d;
        }
    }

    public class TestReportRow
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public bool HasSubItems { get { return this.SubItems != null && this.SubItems.Count > 0; } }
        public List<TestSubRow> SubItems { get; set; }

        public TestReportRow(string Name, int Quantity, decimal Amount)
        {
            this.Name = Name;
            this.Quantity = Quantity;
            this.Amount = Amount;
        }
    }

    public class TestSubRow
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}