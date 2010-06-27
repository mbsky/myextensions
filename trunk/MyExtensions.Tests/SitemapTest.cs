using System;
using System.Web.Google;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
namespace MyExtensions.Tests
{


    /// <summary>
    ///This is a test class for SitemapTest and is intended
    ///to contain all SitemapTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SitemapTest
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
        ///A test for Write
        ///</summary>
        [TestMethod()]
        public void WriteTest()
        {
            string filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sitemap.xml");

            Console.WriteLine(filename);

            Sitemap target = new Sitemap(filename);

            List<MapImage> images = new List<MapImage>();
            images.Add(new MapImage
            {
                Caption = "logo",
                Loc = @"http://www.google.com.hk/intl/zh-CN/images/logo_cn.png",
            });

            target.Add(new SitemapUrl
            {
                Loc = @"http://www.google.com",
                ChangeFreq = ChangeFrequency.Daily,
                LastModifiedDateTime = DateTime.Now,
                Priority = "0.5",
                MapImages = images
            });
            target.Write();
        }
    }
}
