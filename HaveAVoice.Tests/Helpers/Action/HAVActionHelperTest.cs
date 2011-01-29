using System;
using System.Collections.Generic;
using System.Web;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Models.View;
using HaveAVoice.Models.View.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HaveAVoice.Tests.Helpers.Action {
    [TestClass]
    public class HAVActionHelperTest {
        private static int RESTRICTION_ID = 5;
        private Restriction theRestriction;
        private UserInformationModelBuilder theUserInformationModelBuilder;
        private UserPrivacySetting theUserPrivacySettings;

        [TestInitialize]
        public void Initialize() {
            theRestriction = new RestrictionModel.Builder(RESTRICTION_ID)
                .Build()
                .Restriction;

            User myUser = new User();
            theUserInformationModelBuilder = new UserInformationModelBuilder(myUser)
                .SetRestriction(theRestriction);
        }

        [TestMethod]
        public void TestAllowedToPerformAction_MustWaitLonger() {
            HAVPermissionHelper.Reset();
            theRestriction = new RestrictionModel.Builder(RESTRICTION_ID)
                .IssuePostsTimeLimit(100)
                .IssuePostsWaitTimeBeforePostSeconds(15)
                .Build().Restriction;

            theUserInformationModelBuilder.SetRestriction(theRestriction);

            Permission myPermission = Permission.CreatePermission(0, HAVPermission.Post_Issue.ToString(), string.Empty, false);
            theUserInformationModelBuilder.AddPermission(myPermission);
            UserInformationModel myUserInformation = theUserInformationModelBuilder.Build();
            bool allowedToPost = HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Post_Issue);
            Assert.IsTrue(allowedToPost);
            allowedToPost = HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Post_Issue);
            Assert.IsFalse(allowedToPost);
        }

        [TestMethod]
        public void TestAllowedToPerformAction_ReachedLimit() {
            HAVPermissionHelper.Reset();
            theRestriction = new RestrictionModel.Builder(0).IssuePostsWithinTimeLimit(3).IssuePostsTimeLimit(600).Build().Restriction;
            theUserInformationModelBuilder.SetRestriction(theRestriction);
            PermissionTestHelper.AddPermissionToUserInformation(theUserInformationModelBuilder, HAVPermission.Post_Issue);
            UserInformationModel myUserInformation = theUserInformationModelBuilder.Build();

            bool allowedToPost = HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Post_Issue);
            Assert.IsTrue(allowedToPost);
            allowedToPost = HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Post_Issue);
            Assert.IsTrue(allowedToPost);
            allowedToPost = HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Post_Issue);
            Assert.IsTrue(allowedToPost);
            allowedToPost = HAVPermissionHelper.AllowedToPerformAction(myUserInformation, HAVPermission.Post_Issue);
            Assert.IsFalse(allowedToPost);
        }

        [TestMethod]
        public void TestAllowedToPerformAction_NoUserInformation() {
            HAVPermissionHelper.Reset();
            bool allowedToPost = HAVPermissionHelper.AllowedToPerformAction(null, HAVPermission.Post_Issue);
            Assert.IsFalse(allowedToPost);
        }

        [TestMethod]
        public void TestAllowedToPerformAction_AllowedToPostIssue() {
            HAVPermissionHelper.Reset();
            List<Permission> myPermissions = new List<Permission>();
            myPermissions.Add(Permission.CreatePermission(0, HAVPermission.Post_Issue.ToString(), string.Empty, false));
            theUserInformationModelBuilder.AddPermissions(myPermissions);
            
            bool allowedToPost = HAVPermissionHelper.AllowedToPerformAction(theUserInformationModelBuilder.Build(), HAVPermission.Post_Issue);
            
            Assert.IsTrue(allowedToPost);
        }

        [TestMethod]
        public void UnableToExecuteBecauseUserDoesntHavePermission() {
            HAVPermissionHelper.Reset();
            List<Permission> myPermission = new List<Permission>();
            theUserInformationModelBuilder.AddPermissions(myPermission);
            bool myAllowedToPost = HAVPermissionHelper.AllowedToPerformAction(theUserInformationModelBuilder.Build(), HAVPermission.Post_Issue);
            Assert.IsFalse(myAllowedToPost);
        }
    }
}
