using System.Security.Cryptography.X509Certificates;

using MigraDoc.DocumentObjectModel;

namespace cmp.wyam.Pdf.PdfGeneration
{
    public interface IRenderable
    {
        Document RenderOn(Document document);
    }
}