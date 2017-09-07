using cmp.wyam.confluence.Generation;

namespace cmp.wyam.confluence.Documentation
{
    public class PageHeader : IRenderMarkup
    {
        public PageHeader()
        {
        }

        public ConfluencePage RenderOn(ConfluencePage page)
        {
            page.AppendMarkup("{toc:printable=true|style=square|maxLevel=2|indent=40px|minLevel=1|class=bigpink|type=list}");

            return page;
        }
    }
}