using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MyExtensions.Tests
{


    /// <summary>
    ///This is a test class for StringExtensionsMatchTest and is intended
    ///to contain all StringExtensionsMatchTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StringExtensionsMatchTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for GetValueUseRegex
        ///</summary>
        [TestMethod()]
        public void GetValueUseRegexTest()
        {
            string text = @"<div><SPAN class=""sku"">SKU #12345</SPAN></div>";
            string regex = @"<SPAN class=""sku"">SKU [#]?\b\d+\b</SPAN>";
            string groupName = string.Empty;
            string expected = @"<SPAN class=""sku"">SKU #12345</SPAN>";
            string actual;
            actual = StringExtensionsMatch.GetValueUseRegex(text, regex, groupName);
            Assert.AreEqual(expected, actual);

            expected = "#12345";
            regex = @"[#]?\b\d+\b";
            actual = StringExtensionsMatch.GetValueUseRegex(actual, regex, groupName);
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetFirstMatch
        ///</summary>
        [TestMethod()]
        public void GetFirstMatchTest1()
        {
            string text = @"/content/alternate-1.aspx";
            string regex = @"content/([A-Za-z0-9\-]+)\.aspx$";
            string expected = "alternate-1";
            string actual;
            actual = StringExtensionsMatch.GetFirstMatch(text, regex);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFirstMatchTest2()
        {
            string text = @"<div><SPAN class=""sku"">SKU #12345</SPAN></div>";
            string regex = @"<SPAN class=""sku"">SKU #([A-Za-z0-9\-]+)</SPAN>";
            string expected = "12345";
            string actual;
            actual = StringExtensionsMatch.GetFirstMatch(text, regex);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFirstMatchTest3()
        {
            string text = @" ... <a href=""/search/clothing/filter/productTypeFacet/%22Clothing%22/gender/%22womens%22/categoryFacet/%22Tops%22/page/64/sort/goLiveDate/desc/"" class=""pager 64"">65</a>";
            string regex = @">([A-Za-z0-9\-]+)</a>";
            string expected = "65";
            string actual;
            actual = StringExtensionsMatch.GetFirstMatch(text, regex);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetMatches
        ///</summary>
        [TestMethod()]
        public void GetMatchesTest()
        {

            string _source = @"
i'm not running
sanli is running
lufeng is running
xiaotian is running
i'm not running
";
            string pattern = @"([a-z]+)\sis\s([a-z]+)";
            int limit = 0;

            IList<string[]> actual;

            actual = StringExtensionsMatch.GetMatches(_source, pattern, limit);

            int i = 0;
            
            Assert.AreEqual("sanli is running", actual[0][0]);
            Assert.AreEqual("sanli", actual[0][1]);
            Assert.AreEqual("running", actual[0][2]);

        }
    }
}
