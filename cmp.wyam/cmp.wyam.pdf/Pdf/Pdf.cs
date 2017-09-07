using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using cmp.wyam.Pdf.PdfDocumentation;
using cmp.wyam.Pdf.PdfGeneration;

using PdfSharp.Drawing;
using PdfSharp.Pdf;

using Wyam.Common.Documents;
using Wyam.Common.Meta;
using Wyam.Common.Modules;
using Wyam.Common.Pipelines;

namespace cmp.wyam.Pdf
{
    public class Pdf : IModule
    {
        private string title = "Documentation";

        private string pathToLogo;

        private string author;

        public Pdf(string title)
        {
            this.title = title;
        }

        public Pdf(string title, string author)
        {
            this.title = title;
            this.author = author;
        }

        public Pdf(string title, string author, string pathToLogo)
        {
            this.title = title;
            this.author = author;
            this.pathToLogo = pathToLogo;
        }

        public IEnumerable<IDocument> Execute(IReadOnlyList<IDocument> inputs, IExecutionContext context)
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            var namespaceDocuments = inputs.Where(doc => doc.String("SpecificKind") == "Namespace" && doc.String("QualifiedName") != string.Empty)
                                           .OrderBy(doc => doc.String("QualifiedName"));

            var methods = inputs.Where(doc => doc.String("SpecificKind") == "Method");

            IGenerator generator = new PdfDocumentationGenerator(title, author, Path.Combine(context.InputFolder, pathToLogo));

            var stream = generator.GeneratePdf(namespaceDocuments.Select(doc => new Namespace(doc)));

            var pdfDoc = context.GetNewDocument(stream, new List<MetadataItem> { new MetadataItem("RelativeFilePath", "pdf\\doku")  });

            yield return pdfDoc;
        }
    }
}
