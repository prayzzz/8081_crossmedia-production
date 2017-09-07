using Wyam.Common.Documents;

namespace cmp.wyam.confluence.Documentation
{
    internal class Value
    {
        public Value(IDocument doc)
        {
            this.Name = doc.String("FullName");
            this.Description = doc.String("Summary");

            if (this.Description == string.Empty)
            {
                this.Description = " ";
            }
        }

        public string Name { get; private set; }

        public string Description { get; private set; } 
    }
}