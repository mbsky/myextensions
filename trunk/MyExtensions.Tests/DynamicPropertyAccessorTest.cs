using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
namespace MyExtensions.Tests
{
    
    
    /// <summary>
    ///This is a test class for DynamicPropertyAccessorTest and is intended
    ///to contain all DynamicPropertyAccessorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DynamicPropertyAccessorTest
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

        public class Temp
        {
            public int? Value { get; set; }
        }

        /// <summary>
        ///A test for GetValue
        ///</summary>
        ///<remarks>
        /// My Test Result
        /// Reflection: 00:00:01.1640008
        /// Lambda: 00:00:00.1359362
        /// Direct: 00:00:00.0011923
        /// </remarks>
        [TestMethod()]
        public void GetValueTest()
        {
            var t = new Temp { Value = null };

            PropertyInfo propertyInfo = t.GetType().GetProperty("Value");
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var value = propertyInfo.GetValue(t, null);
            }
            watch1.Stop();
            Trace.WriteLine("Reflection: " + watch1.Elapsed);

            DynamicPropertyAccessor property = new DynamicPropertyAccessor(t.GetType(), "Value");
            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var value = property.GetValue(t);
            }
            watch2.Stop();
            Trace.WriteLine("Lambda: " + watch2.Elapsed);

            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            for (var i = 0; i < 1000000; i++)
            {
                var value = t.Value;
            }
            watch3.Stop();
            Trace.WriteLine("Direct: " + watch3.Elapsed);

        }
    }
}
