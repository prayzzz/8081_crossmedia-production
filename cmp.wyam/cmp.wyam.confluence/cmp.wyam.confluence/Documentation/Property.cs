using System.Text.RegularExpressions;
using Wyam.Common.Documents;

namespace cmp.wyam.confluence.Documentation
{
    internal class Property
    {
        public Property(IDocument doc)
        {
            this.Accessibility = doc.String("Accessibility").ToLower();
            this.TypeName = doc.Get<IDocument>("Type").String("DisplayName").Replace("<", "< ");
            this.Name = doc.String("FullName");
            this.Description = Regex.Replace(doc.String("Summary"), "<.*?>", string.Empty).Trim('\n').Trim();

            if (this.Description == string.Empty)
            {
                this.Description = " ";
            }
        }

        public string Accessibility { get; private set; }

        public string TypeName { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}