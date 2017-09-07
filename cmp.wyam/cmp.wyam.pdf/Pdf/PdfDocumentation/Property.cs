using System.Text.RegularExpressions;

using Wyam.Common.Documents;

namespace cmp.wyam.Pdf.PdfDocumentation
{
    internal class Property
    {
        public Property(IDocument doc)
        {
            this.Accessibility = doc.String("Accessibility").ToLower();
            this.TypeName = doc.Get<IDocument>("Type").String("DisplayName").Replace("<", "< ");
            this.TypeId = doc.Get<IDocument>("Type").String("SymbolId");
            this.Name = doc.String("FullName");
            this.Description = Regex.Replace(doc.String("Summary"), "<.*?>", string.Empty).Trim('\n').Trim();
            this.WriteTypeLine = doc.Get<IDocument>("Type").Get<bool>("IsResult");
        }

        public string Accessibility { get; private set; }

        public string TypeName { get; private set; }

        public string TypeId { get; private set; }

        public bool WriteTypeLine { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}