using cmp.wyam.confluence.Documentation;

namespace cmp.wyam.confluence.Generation
{
    public interface IRenderMarkup
    {
        ConfluencePage RenderOn(ConfluencePage page);
    }
}