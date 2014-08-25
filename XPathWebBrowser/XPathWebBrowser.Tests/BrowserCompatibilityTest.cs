using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using XPathWebBrowserComponent.Tests.Properties;

namespace XPathWebBrowserComponent.Tests
{
    [TestClass]
    public class BrowserCompatibilityTest
    {

        [TestInitialize]
        public void Initialize()
        {
            File.WriteAllText("Data.html", Resources.Data);
        }

        [TestMethod]
        public void TestIndexingCorrectness()
        {

            var browser = new XPathWebBrowser();

            var done = false;

            browser.DocumentCompleted += delegate
            {

                var elements = browser
                    .FindElements("//*[@name='blah1']")
                    .ToArray();
                Assert.AreEqual(2, elements.Length);

                browser.Document.Write("<div class='dynamic'>Hello world from dynamic content!</div>");

                var dynamicElements = browser
                    .FindElements("//*[@class='dynamic']")
                    .ToArray();
                Assert.AreEqual(1, dynamicElements.Length);

                done = true;

            };

            browser.Navigate("file://" + Path.Combine(Environment.CurrentDirectory, "Data.html"));

            while (!done)
            {
                Application.DoEvents();
                Thread.Sleep(1);
            }

        }
    }
}
