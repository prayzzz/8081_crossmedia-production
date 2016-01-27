using System.Text.RegularExpressions;
using Wyam.Common.Documents;

namespace cmp.wyam.confluence.Documentation
{
    internal class Method
    {
        public Method(IDocument doc)
        {
            this.Accessibility = doc.String("Accessibility").ToLower();
            this.Name = doc.String("FullName").Replace("<", "< ").Replace("(", "( ");
            this.Description = Regex.Replace(doc.String("Summary"), "<.*?>", string.Empty).Trim('\n').Trim();

            if (this.Description == string.Empty)
            {
                this.Description = " ";
            }

            var typeDoc = doc.Get<IDocument>("ReturnType");

            if (typeDoc != null)
            {
                this.TypeName = typeDoc.String("FullName").Replace("<", "< "); ;
            }
            else
            {
                this.TypeName = "void";
            }
        }

        public string Accessibility { get; private set; }

        public string TypeName { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}