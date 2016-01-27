using System.Collections.Generic;
using System.Linq;
using cmp.wyam.confluence.Documentation;
using cmp.wyam.confluence.Generation;
using Wyam.Common.Documents;
using Wyam.Common.Meta;
using Wyam.Common.Modules;
using Wyam.Common.Pipelines;

namespace cmp.wyam.confluence
{
    using Wyam.Common;

    public class Confluence : IModule
    {
        private readonly string title;

        public Confluence()
        {
        }

        public Confluence(string title)
        {
            this.title = title;
        }

        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            var documents = inputs.Where(doc => doc.String("SpecificKind") == "Namespace" && doc.String("QualifiedName") != string.Empty)
                                           .OrderBy(doc => doc.String("QualifiedName"));

            IGenerateMarkup confluenceMarkupGenerator = new ConfluenceMarkupGenerator();
            var stream = confluenceMarkupGenerator.GenerateConfluencePage(documents.Select(doc => new Namespace(doc)));

            var confluenceDocument = context.GetNewDocument(stream, new List<MetadataItem> { new MetadataItem("RelativeFilePath", this.title) });
            yield return confluenceDocument;
        }
    }
}