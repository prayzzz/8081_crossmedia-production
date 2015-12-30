using System.Collections.Generic;
using System.IO;

namespace cmp.wyam.Pdf.PdfGeneration
{
    public interface IGenerator
    {
        Stream GeneratePdf(IEnumerable<IRenderable> parts);
    }
}