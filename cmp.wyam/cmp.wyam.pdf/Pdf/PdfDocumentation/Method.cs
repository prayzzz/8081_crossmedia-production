using System.Text.RegularExpressions;

using Wyam.Common.Documents;

namespace cmp.wyam.Pdf.PdfDocumentation
{
    internal class Method
    {
        public Method(IDocument doc)
        {
            this.Accessibility = doc.String("Accessibility").ToLower();
            this.Name = doc.String("FullName").Replace("<", "< ").Replace("(", "( ");
            this.Description = Regex.Replace(doc.String("Summary"), "<.*?>", string.Empty).Trim('\n').Trim();

            var typeDoc = doc.Get<IDocument>("ReturnType");

            if (typeDoc != null)
            {
                this.TypeName = typeDoc.String("FullName").Replace("<", "< "); ;
                this.TypeId = typeDoc.String("SymbolId");
                this.WriteTypeLine = typeDoc.Get<bool>("IsResult");
            }
            else
            {
                this.TypeName = "void";
                this.WriteTypeLine = false;
            }
        }

        public string Accessibility { get; private set; }

        public string TypeName { get; private set; }

        public string TypeId { get; private set; }

        public bool WriteTypeLine { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}