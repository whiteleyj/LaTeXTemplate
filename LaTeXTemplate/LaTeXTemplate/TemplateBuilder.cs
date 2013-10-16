namespace LaTeXTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LaTeXTemplate.Components;

    public class TemplateBuilder
    {
        public DocumentComponent BuildTemplateDoc(IEnumerable<string> chunks)
        {
            var leafs = chunks.Select(s => new LeafNode(s));
            var doc = new DocumentComponent();
            doc.SubComponent = BuildComponents(leafs);
            return doc;
        }

        public List<ITemplateComponent> BuildComponents(IEnumerable<LeafNode> nodes)
        {
            var components = new List<ITemplateComponent>();
            var nodeEnumerator = nodes.GetEnumerator();

            while (nodeEnumerator.MoveNext())
            {
                var comp = BuildAComponent(null, nodeEnumerator);
                if (comp != null)
                    components.Add(comp);
            }

            return components;
        }

        public ITemplateComponent BuildAComponent(ITemplateComponent parent, IEnumerator<LeafNode> nodeEnumerator)
        {
            var c = nodeEnumerator.Current;

            // If we've hit an end:if or end:for jump up one level.  
            // If we're at the top just ignore it.
            if (c.IsEndNode && parent != null)
                return null;

            if (c.IsNestedNode)
            {
                ITemplateComponent thisNode = BuildNestNode(c);
                while (nodeEnumerator.MoveNext())
                {
                    // Note: Recursion happening right here.
                    var comp = BuildAComponent(thisNode, nodeEnumerator);
                    if (comp == null)
                        break;

                    thisNode.SubComponent.Add(comp);
                }
                return thisNode;
            }

            if (c.IsTextNode)
                return BuildTextComponent(c);

            if (c.IsLaTeXNode)
                return BuildLaTeXComponent(c);

            return BuildVarComponent(c);
        }

        public LaTeXComponent BuildLaTeXComponent(LeafNode c)
        {
            var tc = new LaTeXComponent();
            tc.CommandName = c.Name;
            tc.SetParamString(c.Params.Aggregate((x, y) => x + y.Trim())); // Join the strings together.            
            return tc;
        }

        public TextComponent BuildTextComponent(LeafNode c)
        {
            return new TextComponent() { TemplateText = c.Text };
        }

        public VarComponent BuildVarComponent(LeafNode c)
        {
            var lastDot = c.Name.LastIndexOf('.');
            if (lastDot > 0)
                c.Name = c.Name.Substring(lastDot + 1);

            return new VarComponent()
            {
                TemplateText = c.Text,
                ValueName = c.Name
            };
        }

        public ITemplateComponent BuildNestNode(LeafNode c)
        {
            if (c.Name.Equals("for"))
                return new ForComponent()
                {
                    CollectionName = c.Params[0],
                    ValueName = c.Params[1],
                    TemplateText = c.Text,
                    SubComponent = new List<ITemplateComponent>()
                };

            if (c.Name.Equals("if"))
                return new IfComponent()
                {
                    BoolName = c.Params[0],
                    TemplateText = c.Text,
                    SubComponent = new List<ITemplateComponent>()
                };

            throw new Exception("Unknown nested template type: " + c.Text);
        }
    }
}