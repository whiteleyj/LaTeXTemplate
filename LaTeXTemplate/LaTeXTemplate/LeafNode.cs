namespace LaTeXTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LeafNode
    {
        // A class that helps read the template.

        public string Text { get; set; }
        public string Name { get; set; }
        public List<string> Params { get; set; }

        public bool IsTextNode { get; private set; }
        public bool IsNestedNode { get { return Name.Equals("for") || Name.Equals("if"); } }
        public bool IsEndNode { get { return Name.Equals("end"); } }
        public bool IsLaTeXNode { get { return Name.StartsWith(@"\"); } }

        public LeafNode(string nodeText)
        {
            this.Name = string.Empty;
            this.Params = new List<string>();
            this.IsTextNode = !nodeText.Trim().StartsWith(TemplateConsts.TMPL_START);
            this.Text = IsTextNode ? nodeText : StripTemplateIndicators(nodeText);

            if (string.IsNullOrEmpty(Text))
                return;

            var s = Text.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            this.Name = s[0].Trim();

            if (s.Length > 1)
                this.Params = s[1].Split(new char[] { ' ' }, 32, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public string StripTemplateIndicators(string nodeText)
        {
            nodeText = nodeText.Trim();
            if (nodeText.StartsWith(TemplateConsts.TMPL_START))
                nodeText = nodeText.Remove(0, TemplateConsts.TMPL_LEN);

            if (nodeText.EndsWith(TemplateConsts.TMPL_END))
                nodeText = nodeText.Remove(nodeText.Length - TemplateConsts.TMPL_LEN);

            return nodeText.Trim();
        }
    }
}