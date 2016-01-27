using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cmp.wyam.confluence.Generation;
using Wyam.Common.Documents;

namespace cmp.wyam.confluence.Documentation
{
    public class Member : IRenderMarkup
    {
        private string name;
        private string type;
        private string description;
        private string syntax;

        private IEnumerable<Method> methods;
        private IEnumerable<Property> properties;
        private IEnumerable<Value> values;

        public Member(IDocument document)
        {
            this.name = document.String("FullName");
            this.type = document.String("SpecificKind");
            this.description = Regex.Replace(document.String("Summary"), "<.*?>", string.Empty);
            this.syntax = document.String("Syntax")
                .Replace(System.Environment.NewLine, "\\\\")
                .Replace("[", "\\[")
                .Replace("]", "\\]");

            this.methods = document.List<IDocument>("Members", new List<IDocument>())
                                   .Where(x => x.Get<bool>("IsResult") && x.String("SpecificKind") == "Method")
                                   .OrderBy(x => x.String("FullName"))
                                   .Select(doc => new Method(doc));

            this.properties = document.List<IDocument>("Members", new List<IDocument>())
                                      .Where(x => x.Get<bool>("IsResult") && x.String("SpecificKind") == "Property")
                                      .OrderBy(x => x.String("FullName"))
                                      .Select(doc => new Property(doc));

            this.values = document.List<IDocument>("Members", new List<IDocument>())
                                  .Where(x => x.Get<bool>("IsResult") && x.String("SpecificKind") == "Field")
                                  .OrderBy(x => x.String("FullName"))
                                  .Select(doc => new Value(doc));
        }

        public ConfluencePage RenderOn(ConfluencePage page)
        {
            page.AppendMarkup($"h2. {this.name} ~{this.type}~");
            page.AppendMarkup($"{description}");
            page.AppendMarkup("h3. Syntax");
            page.AppendMarkup($"bq. {{{{{this.syntax}}}}}");

            if (properties.Any())
            {
                page.AppendMarkup($"h3. Properties");
                page.AppendMarkup($"||Accessibility||Type||Name||Description||");

                foreach (var property in properties)
                {
                    page.AppendMarkup($"|{property.Accessibility}|{property.TypeName}|{property.Name}|{property.Description}|");
                }
            }

            if (methods.Any())
            {
                page.AppendMarkup($"h3. Methods");
                page.AppendMarkup($"||Accessibility||Type||Name||Description||");

                foreach (var method in methods)
                {
                    page.AppendMarkup($"|{method.Accessibility}|{method.TypeName}|{method.Name}|{method.Description}|");
                }
            }

            if (values.Any())
            {
                page.AppendMarkup($"h3. Values");
                page.AppendMarkup($"||Name||Description||");

                foreach (var value in values)
                {
                    page.AppendMarkup($"|{value.Name}|{value.Description}|");
                }
            }

            page.AppendMarkup("----");
            return page;
        }
    }
}