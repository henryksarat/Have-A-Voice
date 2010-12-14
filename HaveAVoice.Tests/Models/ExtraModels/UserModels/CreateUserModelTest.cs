using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Models;
using HaveAVoice.Models.View;

namespace TestProject1.Models.View.UserModels {
    [TestClass]
    public class CreateUserModelBuilderTest {
        private static String theFirstName = "Henryk";
        private static String theMiddleName = "Paul";
        private static String theMiddleNameMultipleSpaces = "P Paul Obaminisha";
        private static String theLastName = "Sarat";

        CreateUserModelBuilder theModelBuilder;

        [TestInitialize]
        public void Initialize() {
            theModelBuilder = new CreateUserModelBuilder();
        }

        [TestMethod]
        public void CorrectTypeReturned() {
            theModelBuilder.FullName("Henryk Sarat");
            Assert.IsInstanceOfType(theModelBuilder.Build(), typeof(User));
        }

        [TestMethod]
        public void TestFirstAndLastNameGiven() {
            String myFullName = theFirstName + " " + theLastName;
            theModelBuilder.FullName(myFullName);

            Assert.AreEqual(theModelBuilder.Build().FirstName, theFirstName);
            Assert.AreEqual(theModelBuilder.Build().MiddleName, String.Empty);
            Assert.AreEqual(theModelBuilder.Build().LastName, theLastName);
        }

        [TestMethod]
        public void TestFirstAndMiddleAndLastNameGiven() {
            String myFullName = theFirstName + " " + theMiddleName + " " + theLastName;
            theModelBuilder.FullName(myFullName);

            Assert.AreEqual(theModelBuilder.Build().FirstName, theFirstName);
            Assert.AreEqual(theModelBuilder.Build().MiddleName, theMiddleName);
            Assert.AreEqual(theModelBuilder.Build().LastName, theLastName);
        }

        [TestMethod]
        public void TestFirstAndMiddleMultipleSpacesAndLastNameGiven() {
            String myFullName = theFirstName + " " + theMiddleNameMultipleSpaces + " " + theLastName;
            theModelBuilder.FullName(myFullName);

            Assert.AreEqual(theModelBuilder.Build().FirstName, theFirstName);
            Assert.AreEqual(theModelBuilder.Build().MiddleName, theMiddleNameMultipleSpaces);
            Assert.AreEqual(theModelBuilder.Build().LastName, theLastName);
        }

        [TestMethod]
        public void TestNoNameGiven() {
            String myFullName = String.Empty;
            theModelBuilder.FullName(myFullName);

            Assert.AreEqual(theModelBuilder.Build().FirstName, String.Empty);
            Assert.AreEqual(theModelBuilder.Build().MiddleName, String.Empty);
            Assert.AreEqual(theModelBuilder.Build().LastName, String.Empty);
        }

        [TestMethod]
        public void TestOnlyFirstNameGiven() {
            String myFullName = theFirstName;
            theModelBuilder.FullName(myFullName);

            Assert.AreEqual(theModelBuilder.Build().FirstName, theFirstName);
            Assert.AreEqual(theModelBuilder.Build().MiddleName, String.Empty);
            Assert.AreEqual(theModelBuilder.Build().LastName, String.Empty);
        }
    }
}
