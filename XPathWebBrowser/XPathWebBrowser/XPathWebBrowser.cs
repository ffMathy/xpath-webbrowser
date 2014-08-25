using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using AgilityHtmlDocument = HtmlAgilityPack.HtmlDocument;

// ReSharper disable once CheckNamespace
public class XPathWebBrowser : WebBrowser
{

    public IEnumerable<HtmlElement> FindElements(string rootXPathQuery)
    {
        var htmlDocument = new AgilityHtmlDocument();

        var documentRootNodes = Document.GetElementsByTagName("*");
        var documentHtml = string.Empty;
        foreach (HtmlElement node in documentRootNodes)
        {
            if (node.Parent == null)
            {
                documentHtml = node.OuterHtml;
            }
        }

        htmlDocument.LoadHtml(documentHtml);

        var documentNode = htmlDocument.DocumentNode;
        if (documentNode != null)
        {

            var nodes = documentNode.SelectNodes(rootXPathQuery);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var equivalent = FindFromNode(node);
                    yield return equivalent;
                }
            }

        }
    }

    private HtmlElement FindFromNode(HtmlNode node)
    {
        var parent = node.ParentNode;

        var childNodesWithSameType =
            parent.ChildNodes.Where(n => string.Equals(n.Name, node.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();

        var parentOffset = childNodesWithSameType.IndexOf(node);

        HtmlElement parentResult;
        if (string.Equals(parent.Name, "BODY", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Assert(Document != null, "Document != null");
            parentResult = Document.Body;
        }
        else
        {
            parentResult = FindFromNode(parent);
        }

        Debug.Assert(parentResult != null, "parentResult != null");

        var childElementsWithSameType = parentResult
            .All
            .Cast<HtmlElement>()
            .Where(e => string.Equals(e.TagName, node.Name, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        return childElementsWithSameType[parentOffset];
    }

}
