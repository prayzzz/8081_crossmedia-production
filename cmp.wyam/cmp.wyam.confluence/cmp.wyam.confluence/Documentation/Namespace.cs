using System;
using System.Collections.Generic;
using System.Linq;
using cmp.wyam.confluence.Generation;
using Wyam.Common.Documents;

namespace cmp.wyam.confluence.Documentation
{
    public class Namespace : IRenderMarkup
    {
        private string name;

        private IEnumerable<Member> members;

        public Namespace(IDocument document)
        {
            this.name = document.String("QualifiedName");
            this.members = document.List<IDocument>("MemberTypes", new List<IDocument>())
                      .Where(x => x.Get<bool>("IsResult") && x.String("Kind") == "NamedType")
                      .OrderBy(x => x.String("FullName"))
                      .Select(doc => new Member(doc));
        }

        public ConfluencePage RenderOn(ConfluencePage page)
        {
            page.AppendMarkup("h1. " + this.name);
            page.AppendMarkup("----");

            return members.Aggregate(page, (current, renderable) => renderable.RenderOn(current));
        }
    }
}