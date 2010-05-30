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

        [TestMethod()]
        public void GetFirstMatchTest4()
        {
            string text = @"/content/alternate-1.aspx";
            string start = @"/content/";
            string end = @".aspx";
            string expected = "alternate-1";
            string actual;
            actual = StringExtensionsMatch.GetFirstMatch(text, start, end, false, false);
            Assert.AreEqual(expected, actual);
            

            expected = @"/content/alternate-1";
            actual = StringExtensionsMatch.GetFirstMatch(text, start, end, true, false);
            Assert.AreEqual(expected, actual);

            text = "List Price</font></a></td><td align=left><font face=\"arial\" size=\"2\">$795.00</font></td><td><font color=\"#C61344\" face=arial size=3>&nbsp; &nbsp;  only";
            start = "List Price</font></a></td><td align=left><font face=\"arial\" size=\"2\">";
            end = "<font color=\"#C61344\" face=arial size=3>&nbsp; &nbsp;  only";
            expected = "$795.00</font></td><td>";
            actual = StringExtensionsMatch.GetFirstMatch(text, start, end, false, false);
            Assert.AreEqual(expected, actual);

            text = @"
      <option value=""> size </option>
      
                  
	      <option> XS (0-2) </option> 
	         
	      <option> S (4-6) </option> 
	         
	      <option> M (8-10) </option> 
	         
	      <option> L (12-14) </option> 
	         
	      <option> XL (16-18) </option> 
	          
    </select>
            ";
            start = @"<option value=""> size </option>";
            end = @"</select>";
            expected = @"
      
                  
	      <option> XS (0-2) </option> 
	         
	      <option> S (4-6) </option> 
	         
	      <option> M (8-10) </option> 
	         
	      <option> L (12-14) </option> 
	         
	      <option> XL (16-18) </option> 
	          
    ";
            actual = StringExtensionsMatch.GetFirstMatch(text, start, end, false, false);
            Assert.AreEqual(expected, actual);

            text = @"<!-- Display a drop-down list box for each attribute --><select name=""size_0"" id=""size_0"" style=""""onChange=""""class=""sel-size""><option value=""""> size </option><option> 32A </option> <option> 32B </option> <option> 32C </option> <option> 32D </option> <option> 32DD </option> <option> 34A </option> <option> 34B </option> <option> 34C </option> <option> 34D </option> <option> 34DD </option> <option> 36A </option> <option> 36B </option> <option> 36C </option> <option> 36D </option> <option> 36DD </option> <option> 38A </option> <option> 38B </option> <option> 38C </option> <option> 38D </option> <option> 38DD </option> <option> 40C </option> <option> 40D </option> <option> 40DD </option> </select><select name=""color_0"" id=""color_0"" style=""""onChange=""""class=""sel-color""><option value="""">  color </option><option value=""367-buff""> buff </option> <option value=""h28-evening blush""> evening blush </option> <option value=""h32-shell pink""> shell pink </option> <option value=""h90-red""> red </option> <option value=""092-white""> white </option> <option value=""c07-heather charcoal""> heather charcoal </option> <option value=""dl3-black""> black </option> </select><!-- Quantity is an attribute that is hardcoded --><select name=""quantity_0""style=""""onChange=""""class=""sel-quantity""><option value=""""> quantity </option><option> 1 </option><option> 2 </option><option> 3 </option><option> 4 </option><option> 5 </option></select></div><!-- end droplist div --><div id=""atp-msg-0"" class=""atp-msg""><div class=""atp-msg-window""><span class=""atp-msg-cntnr""></span>";

            start = @"<option value=""""> size </option>";
            end = @"</select>";
            expected = @"<option> 32A </option> <option> 32B </option> <option> 32C </option> <option> 32D </option> <option> 32DD </option> <option> 34A </option> <option> 34B </option> <option> 34C </option> <option> 34D </option> <option> 34DD </option> <option> 36A </option> <option> 36B </option> <option> 36C </option> <option> 36D </option> <option> 36DD </option> <option> 38A </option> <option> 38B </option> <option> 38C </option> <option> 38D </option> <option> 38DD </option> <option> 40C </option> <option> 40D </option> <option> 40DD </option> ";

            actual = StringExtensionsMatch.GetFirstMatch(text, start, end, false, false);
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
