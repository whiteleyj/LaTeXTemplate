namespace LaTeXTemplate.Components
{
    using System.Collections.Generic;

    public interface ITemplateComponent
    {
        List<ITemplateComponent> SubComponent { get; set; }
        string TemplateText { get; set; }
        string Render(DataChain obj);
    }
}
