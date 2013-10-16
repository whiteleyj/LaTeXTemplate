namespace LaTeXTemplate.Components
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ForComponent : ITemplateComponent
    {
        public List<ITemplateComponent> SubComponent { get; set; }
        public string TemplateText { get; set; }
        public string ValueName;
        public string CollectionName;

        public string Render(DataChain obj)
        {
            var output = new StringBuilder();
            var collection = obj.GetCollection(CollectionName);
            var thisData = AddLinkToChain(obj);

            foreach (var item in collection)
            {
                thisData.CurrentObj = item;
                SubComponent.ForEach(c => output.Append(c.Render(thisData)));
            }
            return output.ToString();
        }

        private DataChain AddLinkToChain(DataChain chain)
        {
            Type typeOfCollection = chain.GetCollectionsType(CollectionName);
            var link = new DataChain(typeOfCollection);
            link.Prev = chain;
            link.CurrentValName = ValueName;
            return link;
        }
    }
}