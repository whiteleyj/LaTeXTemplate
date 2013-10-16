namespace LaTeXTemplate
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class TexToPdf
    {
        public static string PdfLatexExecutable = @"\bin\pdflatex.exe";

        public string InputPath { get; set; }
        public string OutputPath { get; private set; }
        public string DropPath { get; set; }

        public string OutputLog { get; private set; }


        // Files that are in play.

        public TexToPdf()
        {
            CheckTeXPath();
        }

        public void Convert(string inputPath = "", string outputPath = "")
        {
            CheckParameters(inputPath, outputPath);
            RunTheConverter();
            CleanUpAfterPdfLatex();
        }

        private void CheckParameters(string inputPath, string outputPath)
        {
            if (!string.IsNullOrWhiteSpace(inputPath))
                this.InputPath = inputPath;

            if (!string.IsNullOrWhiteSpace(outputPath))
                this.OutputPath = outputPath;

            if (string.IsNullOrWhiteSpace(this.InputPath) || !File.Exists(InputPath))
                throw new FileNotFoundException("Can't find the input file at: " + inputPath);

            if (string.IsNullOrWhiteSpace(this.OutputPath))
                throw new FileNotFoundException("Output path is not specified.");

            if (File.Exists(OutputPath))
                File.Delete(OutputPath);

            DropPath = Path.GetDirectoryName(InputPath) + "\\"
                         + Path.GetFileName(InputPath).Replace(".tex", ".pdf");
        }

        private void RunTheConverter()
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = PdfLatexExecutable;
            p1.StartInfo.Arguments = string.Format(
                            "-synctex=1 -interaction=nonstopmode -output-directory {1} \"{0}\"",
                            this.InputPath,
                            Path.GetDirectoryName(this.InputPath));
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p1.StartInfo.RedirectStandardOutput = true;
            p1.StartInfo.UseShellExecute = false;

            p1.Start();
            this.OutputLog = p1.StandardOutput.ReadToEnd();
            p1.WaitForExit();
        }

        public void Open()
        {
            if (File.Exists(OutputPath))
                Process.Start(OutputPath);
        }

        private void CheckTeXPath()
        {
            if (!File.Exists(PdfLatexExecutable))
                throw new FileNotFoundException(
                    "pdflatex.exe could not be found at the specified path. \n" +
                     "Check your configuration file under the pdflatex entry.",
                     PdfLatexExecutable);
        }

        private void CleanUpAfterPdfLatex()
        {
            if (!File.Exists(this.DropPath))
                throw new FileNotFoundException("The pdf was not generated.",
                                                this.OutputPath,
                                                new Exception(this.OutputLog));

            var safeDel = new Action<string>(f => { if (File.Exists(f)) { File.Delete(f); } });

            if (DropPath.ToLower() != OutputPath.ToLower())
                safeDel(this.OutputPath);

            File.Move(DropPath, OutputPath);

            safeDel(InputPath.Replace(".tex", ".aux"));
            safeDel(InputPath.Replace(".tex", ".log"));
            safeDel(InputPath.Replace(".tex", ".synctex.gz"));

            if (File.Exists(OutputPath))
                safeDel(InputPath);
        }
    }
}


