using System.Collections.Generic;
using System.IO;
using System.Linq;

using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace cmp.wyam.Pdf.PdfGeneration
{
    public abstract class BaseGenerator : IGenerator
    {
        public Stream GeneratePdf(IEnumerable<IRenderable> parts)
        {
            Document document = parts.Aggregate(PrepareDocument(), (current, renderable) => renderable.RenderOn(current));

            var renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;

            renderer.RenderDocument();

            var stream = new MemoryStream();
            renderer.PdfDocument.Save(stream);

            return stream;
        }

        protected abstract Document PrepareDocument();
    }
}