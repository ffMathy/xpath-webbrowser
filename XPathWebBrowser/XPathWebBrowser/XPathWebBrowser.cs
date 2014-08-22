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
        htmlDocument.LoadHtml(DocumentText);

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
        if (string.Equals(node.Name, "BODY", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Assert(Document != null, "Document != null");
            return Document.Body;
        }
        else
        {
            var parentOffset = parent.ChildNodes.IndexOf(node);
            var parentResult = FindFromNode(parent);

            return parentResult.All[parentOffset];
        }
    }
}
