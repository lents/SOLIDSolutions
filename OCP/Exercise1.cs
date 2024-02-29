using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCP
{
    using System;
    using System.Collections.Generic;

    // Interface for report generation strategy
    public interface IReportGenerationStrategy
    {
        void GenerateReport(List<string> data);
    }

    // PDF report generation strategy
    public class PDFReportGenerationStrategy : IReportGenerationStrategy
    {
        public void GenerateReport(List<string> data)
        {
            // Code to generate PDF report
            Console.WriteLine("Generating PDF report...");
        }
    }

    // Excel report generation strategy
    public class ExcelReportGenerationStrategy : IReportGenerationStrategy
    {
        public void GenerateReport(List<string> data)
        {
            // Code to generate Excel report
            Console.WriteLine("Generating Excel report...");
        }
    }

    // CSV report generation strategy
    public class CSVReportGenerationStrategy : IReportGenerationStrategy
    {
        public void GenerateReport(List<string> data)
        {
            // Code to generate CSV report
            Console.WriteLine("Generating CSV report...");
        }
    }

    // Class responsible for generating reports
    public class ReportGenerator
    {
        private readonly IReportGenerationStrategy _strategy;

        public ReportGenerator(IReportGenerationStrategy strategy)
        {
            _strategy = strategy;
        }

        public void GenerateReport(List<string> data)
        {
            _strategy.GenerateReport(data);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Creating report generator with PDF report strategy
            ReportGenerator pdfReportGenerator = new ReportGenerator(new PDFReportGenerationStrategy());
            pdfReportGenerator.GenerateReport(new List<string> { "Data1", "Data2", "Data3" });

            // Creating report generator with Excel report strategy
            ReportGenerator excelReportGenerator = new ReportGenerator(new ExcelReportGenerationStrategy());
            excelReportGenerator.GenerateReport(new List<string> { "Data1", "Data2", "Data3" });

            // Creating report generator with CSV report strategy
            ReportGenerator csvReportGenerator = new ReportGenerator(new CSVReportGenerationStrategy());
            csvReportGenerator.GenerateReport(new List<string> { "Data1", "Data2", "Data3" });
        }
    }
}
