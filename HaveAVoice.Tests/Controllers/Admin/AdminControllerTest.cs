using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HaveAVoice.Controllers.Admin;
using HaveAVoice.Tests.Helpers;
using HaveAVoice.Helpers;
using System.Web.Mvc;

namespace HaveAVoice.Tests.Controllers.Admin {
    [TestClass]
    public class AdminControllerTest : ControllerTestCase {
        /*

            private static AdminController theController;

            [TestInitialize]
            public void Initialize() {
                theController = new AdminController(theMockedBaseService.Object);
                theController.ControllerContext = GetControllerContext();
            }

            [TestMethod]
            public void ShouldLoadAdminMainPage() {
                PermissionTestHelper.AddPermissionToUserInformation(theUserInformationBuilder, HAVPermission.View_Admin);

                var myResult = theController.Index() as ViewResult;

                AssertAuthenticatedCleanSuccess(myResult, "Index");
            }

            [TestMethod]
            public void ShouldNotLoadAdminMainPageWithoutPermission() {
                var myResult = theController.Index() as ViewResult;

                AssertAuthenticatedRedirection(myResult);
            }

            [TestMethod]
            public void ShouldNotLoadAdminMainPageNotLoggedIn() {
                MockNotLoggedIn();

                var myResult = theController.Index() as ViewResult;

                AssertAuthenticatedRedirection(myResult);
            }

            protected override Controller GetController() {
                return theController;
            }
         */
    }
}
