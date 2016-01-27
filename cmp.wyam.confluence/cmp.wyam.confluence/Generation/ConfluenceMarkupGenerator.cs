using System.Collections.Generic;
using System.IO;
using System.Linq;
using cmp.wyam.confluence.Documentation;

namespace cmp.wyam.confluence.Generation
{
    public class ConfluenceMarkupGenerator : IGenerateMarkup
    {
        public Stream GenerateConfluencePage(IEnumerable<IRenderMarkup> parts)
        {
            var confluencePage = parts.Aggregate(PrepareHeader(),
                (current, renderMarkup) => renderMarkup.RenderOn(current));

            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);

            streamWriter.Write(confluencePage.Content);
            streamWriter.Flush();

            stream.Position = 0;
            return stream;
        }

        private ConfluencePage PrepareHeader()
        {
            return new PageHeader().RenderOn(new ConfluencePage());
        }
    }
}