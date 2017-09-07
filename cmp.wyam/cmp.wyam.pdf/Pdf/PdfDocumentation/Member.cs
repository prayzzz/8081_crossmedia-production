using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using cmp.wyam.Pdf.PdfGeneration;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

using Wyam.Common.Documents;

namespace cmp.wyam.Pdf.PdfDocumentation
{
    public class Member : IRenderable
    {
        private string name;

        private string id;

        private string type;

        private string description;

        private string syntax;

        private IEnumerable<Property> properties;

        private IEnumerable<Method> methods;

        private IEnumerable<Value> values;

        public Member(IDocument document)
        {
            this.name = document.String("FullName");
            this.id = document.String("SymbolId");
            this.type = document.String("SpecificKind");
            this.description = Regex.Replace(document.String("Summary"), "<.*?>", string.Empty);
            this.syntax = document.String("Syntax");

            this.properties = document.List<IDocument>("Members", new List<IDocument>())
                                      .Where(x => x.Get<bool>("IsResult") && x.String("SpecificKind") == "Property")
                                      .OrderBy(x => x.String("FullName"))
                                      .Select(doc => new Property(doc));

            this.methods = document.List<IDocument>("Members", new List<IDocument>())
                                   .Where(x => x.Get<bool>("IsResult") && x.String("SpecificKind") == "Method")
                                   .OrderBy(x => x.String("FullName"))
                                   .Select(doc => new Method(doc));

            this.values = document.List<IDocument>("Members", new List<IDocument>())
                                  .Where(x => x.Get<bool>("IsResult") && x.String("SpecificKind") == "Field")
                                  .OrderBy(x => x.String("FullName"))
                                  .Select(doc => new Value(doc));
        }

        public virtual Document RenderOn(Document document)
        {
            Section tocSection = document.Sections[0];

            Paragraph paragraph = tocSection.AddParagraph();
            paragraph.Style = "TOC2";
            Hyperlink hyperlink = paragraph.AddHyperlink(id);
            hyperlink.AddText("     " + name + "\t");
            hyperlink.AddPageRefField(id);

            Section contentSection = document.LastSection;
            paragraph = contentSection.AddParagraph(name + " - " + type, "Heading2");
            paragraph.AddBookmark(id);

            if (!string.IsNullOrWhiteSpace(description))
            {
                paragraph = contentSection.AddParagraph(description);
            }

            paragraph = contentSection.AddParagraph("Syntax", "Heading4");
            paragraph = contentSection.AddParagraph(syntax, "Code");

            if (properties.Any())
            {
                RenderProperties(contentSection);
            }

            if (methods.Any())
            {
                RenderMethods(contentSection);
            }

            if (values.Any())
            {
                RenderValues(contentSection);
            }

            paragraph = contentSection.AddParagraph();
            paragraph.Format.Borders.Bottom.Color = Colors.Black;
            paragraph.Format.Borders.Bottom.Style = BorderStyle.Single;
            paragraph.Format.SpaceAfter = "2cm";

            return document;
        }

        private void RenderValues(Section contentSection)
        {
            var paragraph = contentSection.AddParagraph("Values", "Heading4");

            var table = this.PrepareSmallTable(contentSection);

            foreach (var value in values)
            {
                // Each item fills two rows
                Row row = table.AddRow();
                row.TopPadding = 1.5;
                row.BottomPadding = 1.5;

                row.Cells[0].AddParagraph(value.Name);
                row.Cells[1].AddParagraph(value.Description);

                table.SetEdge(0, 0, 2, 1, Edge.Box, BorderStyle.Single, 0.75);
            }
        }

        private Table PrepareSmallTable(Section contentSection)
        {
            var table = contentSection.AddTable();
            table.Style = "Table";
            table.Borders.Color = Colors.Black;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("8cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightBlue;
            row.Cells[0].AddParagraph("Name");
            row.Cells[1].AddParagraph("Description");

            table.SetEdge(0, 0, 2, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            return table;
        }

        private void RenderMethods(Section contentSection)
        {
            var paragraph = contentSection.AddParagraph("Methods", "Heading4");

            var table = this.PrepareBigTable(contentSection);

            foreach (var method in methods)
            {
                // Each item fills two rows
                Row row = table.AddRow();
                row.TopPadding = 1.5;
                row.BottomPadding = 1.5;

                row.Cells[0].AddParagraph(method.Accessibility);

                paragraph = row.Cells[1].AddParagraph();
                if (method.WriteTypeLine)
                {
                    Hyperlink hyperlink = paragraph.AddHyperlink(method.TypeId);
                    hyperlink.AddText(method.TypeName);
                }
                else
                {
                    paragraph.AddText(method.TypeName);
                }

                row.Cells[2].AddParagraph(method.Name);
                row.Cells[3].AddParagraph(method.Description);

                table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 0.75);
            }
        }

        private void RenderProperties(Section contentSection)
        {
            var paragraph = contentSection.AddParagraph("Properties", "Heading4");

            var table = this.PrepareBigTable(contentSection);

            foreach (var property in properties)
            {
                // Each item fills two rows
                Row row = table.AddRow();
                row.TopPadding = 1.5;
                row.BottomPadding = 1.5;

                row.Cells[0].AddParagraph(property.Accessibility);

                paragraph = row.Cells[1].AddParagraph();
                if (property.WriteTypeLine)
                {
                    Hyperlink hyperlink = paragraph.AddHyperlink(property.TypeId);
                    hyperlink.AddText(property.TypeName);
                }
                else
                {
                    paragraph.AddText(property.TypeName);
                }

                row.Cells[2].AddParagraph(property.Name);
                row.Cells[3].AddParagraph(property.Description);

                table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 0.75);
            }
        }

        private Table PrepareBigTable(Section contentSection)
        {
            var table = contentSection.AddTable();
            table.Style = "Table";
            table.Borders.Color = Colors.Black;
            table.Borders.Width = 0.25;
            table.Borders.Left.Width = 0.5;
            table.Borders.Right.Width = 0.5;
            table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            Column column = table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("4cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = Colors.LightBlue;
            row.Cells[0].AddParagraph("Accessibility");
            row.Cells[1].AddParagraph("Type");
            row.Cells[2].AddParagraph("Name");
            row.Cells[3].AddParagraph("Description");

            table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
            return table;
        }
    }
}