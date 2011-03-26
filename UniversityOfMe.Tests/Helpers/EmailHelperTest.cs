using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversityOfMe.Helpers;

namespace UniversityOfMe.Tests.Helpers {
    [TestClass]
    public class EmailHelperTest {
        [TestMethod]
        public void TestMethod1() {
            string myActual = EmailHelper.ExtractEmailExtension("henryk.sarat@uchicago.edu");
            Assert.AreEqual(myActual, "uchicago.edu");
        }
    }
}
