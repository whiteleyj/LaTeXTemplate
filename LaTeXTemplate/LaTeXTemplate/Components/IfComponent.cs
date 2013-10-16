namespace LaTeXTemplate.Components
{
    using System.Collections.Generic;
    using System.Text;

    public class IfComponent : ITemplateComponent
    {
        public List<ITemplateComponent> SubComponent { get; set; }
        public string TemplateText { get; set; }
        public string BoolName { get; set; }

        public string Render(DataChain obj)
        {
            if (!obj.GetValue(BoolName).ToLower().Contains("true"))
                return "";      // if it is not a string with the word true in it.

            var sb = new StringBuilder();
            SubComponent.ForEach(s => sb.Append(s.Render(obj)));
            return sb.ToString();
        }
    }
}