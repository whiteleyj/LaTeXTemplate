namespace LaTeXTemplate.Components
{
    using System.Collections.Generic;

    public class VarComponent : ITemplateComponent
    {
        public List<ITemplateComponent> SubComponent { get; set; }
        public string TemplateText { get; set; }
        public string ValueName { get; set; }

        public string Render(DataChain obj)
        {
            return TemplateConsts.LaTeXEscape(obj.GetValue(this.ValueName));
        }
    }
}