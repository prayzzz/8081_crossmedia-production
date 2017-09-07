using System;

namespace cmp.wyam.confluence.Documentation
{
    public class ConfluencePage
    {
        public string Content { get; private set; }

        public void AppendMarkup(string markup)
        {
            this.Content += markup;
            this.Content += Environment.NewLine;
        }
    }
}