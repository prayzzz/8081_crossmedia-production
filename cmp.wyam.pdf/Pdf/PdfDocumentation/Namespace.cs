using System.Collections.Generic;
using System.Linq;

using cmp.wyam.Pdf.PdfGeneration;

using MigraDoc.DocumentObjectModel;

using Wyam.Common.Documents;

namespace cmp.wyam.Pdf.PdfDocumentation
{
    public class Namespace : IRenderable
    {
        private string name;

        private string id;

        private IEnumerable<Member> members;

        public Namespace(IDocument document)
        {
            this.name = document.String("QualifiedName");
            this.id = document.String("SymbolId");

            members = document.List<IDocument>("MemberTypes", new List<IDocument>())
                      .Where(x => x.Get<bool>("IsResult") && x.String("Kind") == "NamedType" )
                      .OrderBy(x => x.String("FullName"))
                      .Select(doc => new Member(doc));
        }

        public Document RenderOn(Document document)
        {
            Section tocSection = document.Sections[0];

            Paragraph paragraph = tocSection.AddParagraph();
            paragraph.Style = "TOC1";
            Hyperlink hyperlink = paragraph.AddHyperlink(id);
            hyperlink.AddText(name + "\t");
            hyperlink.AddPageRefField(id);

            Section contentSection = document.LastSection;
            paragraph = contentSection.AddParagraph(name, "Heading1");
            paragraph.AddBookmark(id);

            return members.Aggregate(document, (current, renderable) => renderable.RenderOn(current));
        }
    }
}