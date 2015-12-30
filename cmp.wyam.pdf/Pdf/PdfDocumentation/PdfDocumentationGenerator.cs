using System.Collections.Generic;
using System.IO;

using cmp.wyam.Pdf.PdfGeneration;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using System;

namespace cmp.wyam.Pdf.PdfDocumentation
{
    public class PdfDocumentationGenerator : BaseGenerator
    {
        private readonly string author;

        private readonly string pathToLogo;

        private readonly string title;

        public PdfDocumentationGenerator(string title, string author)
        {
            this.author = author;
            this.title = title;
        }

        public PdfDocumentationGenerator(string title, string author, string pathToLogo)
        {
            this.author = author;
            this.pathToLogo = pathToLogo;
            this.title = title;
        }


        protected override Document PrepareDocument()
        {
            Document doc = new Document();

            doc = this.DefineStyles(doc);
            doc = this.CreateFirstSection(doc);
            doc = this.CreateContentSection(doc);

            return doc;
        }

        private Document CreateContentSection(Document doc)
        {
            Section section = doc.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            section.PageSetup.StartingNumber = 1;

            HeaderFooter header = section.Headers.Primary;
            header.AddParagraph("\t" + DateTime.Today.ToShortDateString());

            header = section.Headers.EvenPage;
            header.AddParagraph(title);

            // Create a paragraph with centered page number. See definition of style "Footer".
            Paragraph paragraph = new Paragraph();
            paragraph.AddTab();
            paragraph.AddPageField();

            // Add paragraph to footer for odd pages.
            section.Footers.Primary.Add(paragraph);
            // Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
            // not belong to more than one other object. If you forget cloning an exception is thrown.
            section.Footers.EvenPage.Add(paragraph.Clone());

            return doc;
        }

        private Document DefineStyles(Document doc)
        {
            // Get the predefined style Normal.
            Style style = doc.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = doc.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = Colors.DarkBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;

            style = doc.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;

            style = doc.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;


            style = doc.Styles["Heading4"];
            style.ParagraphFormat.OutlineLevel = OutlineLevel.BodyText;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = doc.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = doc.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = doc.Styles.AddStyle("Code", "Normal");
            style.Font.Name = "Consolas";

            // Create a new style called TOC based on style Normal
            style = doc.Styles.AddStyle("TOC1", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;
            style.ParagraphFormat.SpaceBefore = "0.5cm";

            style = doc.Styles.AddStyle("TOC2", "Normal");
            style.ParagraphFormat.LeftIndent = "1cm";
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;

            // Create a new style called Table based on style Normal
            style = doc.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            return doc;
        }

        private Document CreateFirstSection(Document doc)
        {
            // Cover
            Section section = doc.AddSection();

            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.SpaceAfter = "3cm";

            if (!string.IsNullOrWhiteSpace(pathToLogo))
            {
                Image image = section.AddImage(pathToLogo);
                image.Width = "10cm";
            }

            paragraph = section.AddParagraph(title);
            paragraph.Format.Font.Size = 32;
            paragraph.Format.Font.Color = Colors.DarkRed;
            paragraph.Format.SpaceBefore = "8cm";
            paragraph.Format.SpaceAfter = "1cm";

            paragraph = section.AddParagraph("Author: " + author);
            paragraph.Format.Font.Size = 24;
            paragraph.Format.Font.Color = Colors.DarkBlue;
            paragraph.Format.SpaceAfter = "1cm";

            paragraph = section.AddParagraph("Rendering date: ");
            paragraph.Format.SpaceBefore = "2cm";
            paragraph.AddDateField();

            // Table Of Contents
            section.AddPageBreak();

            paragraph = section.AddParagraph("Table of Contents");
            paragraph.Format.Font.Size = 14;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.SpaceAfter = 24;
            paragraph.Format.OutlineLevel = OutlineLevel.Level1;

            return doc;
        }
    }
}