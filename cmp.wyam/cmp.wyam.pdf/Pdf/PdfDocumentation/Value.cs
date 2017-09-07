using Wyam.Common.Documents;

namespace cmp.wyam.Pdf.PdfDocumentation
{
    internal class Value
    {
        public Value(IDocument doc)
        {
            this.Name = doc.String("FullName");
            this.Description = doc.String("Summary");
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}