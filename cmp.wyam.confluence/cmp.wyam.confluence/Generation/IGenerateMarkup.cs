using System.Collections.Generic;
using System.IO;
using cmp.wyam.confluence.Documentation;

namespace cmp.wyam.confluence.Generation
{
    public interface IGenerateMarkup
    {
        Stream GenerateConfluencePage(IEnumerable<IRenderMarkup> parts);
    }
}