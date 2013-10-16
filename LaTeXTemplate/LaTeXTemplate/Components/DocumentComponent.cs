namespace LaTeXTemplate.Components
{
    using System.Collections.Generic;
    using System.Text;

    public class DocumentComponent : ITemplateComponent
    {
        public List<ITemplateComponent> SubComponent { get; set; }
        public string TemplateText { get; set; }

        public string Render(object obj)
        {
            DataChain d = new DataChain(obj.GetType());
            d.CurrentObj = obj;
            return Render(d);
        }

        public string Render(DataChain obj)
        {
            var sb = new StringBuilder();
            SubComponent.ForEach(c => sb.Append(c.Render(obj)));
            return sb.ToString();
        }
    }
}