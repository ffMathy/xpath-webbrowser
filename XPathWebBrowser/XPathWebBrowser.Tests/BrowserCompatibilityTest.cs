using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XPathWebBrowserComponent.Tests
{
    [TestClass]
    public class BrowserCompatibilityTest
    {

        private const string TestHtml =
@"<html>
    <head></head>
    <body>
        <div name='blah1'>
            This is cool
            <div name='subblah1'>This is also cool</div>
            <div name='subblah2'>Blub blab</div>
        </div>
        <div name='blah2'>
            This is also cool
            <div name='subblah1'>This is also cool</div>
            <div name='subblah2'>Blub blab</div>
        </div>
        <b name='hello1'>And so is this</b>
        <b name='blah1'>And this</b>
    </body>
</html>";

        [TestMethod]
        public void TestIndexingCorrectness()
        {

            var browser = new XPathWebBrowser();
            browser.DocumentText = TestHtml;

            var elements = browser
                .FindElements("//*[@name='blah1']")
                .ToArray();
            Assert.AreEqual(2, elements.Length);


        }
    }
}
