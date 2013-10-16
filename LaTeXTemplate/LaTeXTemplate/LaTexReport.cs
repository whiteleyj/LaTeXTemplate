namespace LaTeXTemplate
{
    using System.IO;
    using LaTeXTemplate.Components;
    
    /// <summary>
    /// A suggested implementation of the template library.
    /// </summary>
    /// <typeparam name="T">The data type of the template.</typeparam>
    public abstract class LaTeXReport<T>
    {
        protected DocumentComponent _doc;
        protected DataChain _datachain;
        protected TexToPdf _converter;

        protected abstract string GetWorkingDirectory();
        protected abstract string GetDestinationBasePath();
        protected abstract string GetTemplatePath();
        protected abstract string GetRelativeFilePath();

        public void Init()
        {
            var reader = new TemplateReader(GetTemplatePath());
            var builder = new TemplateBuilder();
            _doc = builder.BuildTemplateDoc(reader.GetTemplateChunks());
            _datachain = new DataChain(typeof(T));
            _converter = new TexToPdf();
        }

        public void BuildReport(T dataModel, bool openAfterCreate = false)
        {
            _datachain.CurrentObj = dataModel;
            string reportData = _doc.Render(_datachain);

            string filePath = GetRelativeFilePath();
            string workingPath = Path.Combine(GetWorkingDirectory(), filePath);
            string destinationPath = Path.Combine(GetDestinationBasePath(), filePath);

            File.WriteAllText(workingPath, reportData);
            _converter.Convert(workingPath, destinationPath);

            if (openAfterCreate)
                _converter.Open();
        }
    }
}
