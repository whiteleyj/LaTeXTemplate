namespace LaTeXTemplate.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LaTeXComponent : ITemplateComponent
    {
        public List<ITemplateComponent> SubComponent { get; set; }
        public string TemplateText { get; set; }
        public string CommandName { get; set; }

        public List<string> Params;

        public void SetParamString(string parameters)
        {
            // Splits on the end bracket and removes the start bracket.
            Params = parameters.Split(new char[] { '}' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => p.Replace("{", string.Empty).Trim())
                               .ToList();
        }

        public string Render(DataChain obj)
        {
            var sb = new StringBuilder();
            sb.Append(this.CommandName);
            Params.ForEach(p => sb.Append(string.Format("{{{0}}}", TemplateConsts.LaTeXEscape(obj.GetValue(p)))));
            return sb.ToString();
        }
    }
}