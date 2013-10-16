namespace LaTeXTemplate.Components
{
    using System.Collections.Generic;

    public class TextComponent : ITemplateComponent
    {
        public List<ITemplateComponent> SubComponent { get; set; }
        public string TemplateText { get; set; }
        public string Render(DataChain obj)
        {
            return TemplateText;
        }
    }
}