using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.Issues;
using Social.Generic.Models;
using Social.Validation;

namespace HaveAVoice.Models.View {
    public class IssueModelBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            IHAVIssueService issueService = new HAVIssueService(new ModelStateWrapper(null));
            IHAVIssueReplyService issueReplyService = new HAVIssueReplyService(new ModelStateWrapper(null));

            UserInformationModel<User> myUser = HAVUserInformationFactory.GetUserInformation();

            int issueId = Int32.Parse(BinderHelper.GetA(aBindingContext, "IssueId"));
            int myTotalAgrees = Int32.Parse(BinderHelper.GetA(aBindingContext, "TotalAgrees"));
            int myTotalDisagrees = Int32.Parse(BinderHelper.GetA(aBindingContext, "TotalDisagrees"));

            string reply = BinderHelper.GetA(aBindingContext, "Reply");

            Issue issue = issueService.GetIssue(issueId, myUser);

            List<string> myRegisteredRoles = new List<string>();
            myRegisteredRoles.Add(Roles.OFFICIAL);
            myRegisteredRoles.Add(Roles.REGISTERED);
            IEnumerable<IssueReplyModel> registeredUserReplys = issueReplyService.GetReplysToIssue(myUser.Details, issue, myRegisteredRoles, PersonFilter.People);

            List<string> myOfficialRoles = new List<string>();
            myOfficialRoles.Add(Roles.OFFICIAL);
            IEnumerable<IssueReplyModel> officialUserReplys = issueReplyService.GetReplysToIssue(myUser.Details, issue, myOfficialRoles, PersonFilter.Politicians);
            bool anonymous = BinderHelper.GetA(aBindingContext, "Anonymous") == "true,false" ? true : false;
            string extractedDisposition = BinderHelper.GetA(aBindingContext, "Disposition");
            Disposition dispotion;
            if(extractedDisposition.Equals("")) {
                dispotion = Disposition.None;
            } else {
                dispotion = (Disposition)Enum.Parse(typeof(Disposition), extractedDisposition, true);
            }

            List<IssueReplyModel> myMerged = new List<IssueReplyModel>();
            myMerged.AddRange(registeredUserReplys);
            myMerged.AddRange(officialUserReplys);
            IssueModel issueModel = new IssueModel(issue, myMerged);
            issueModel.Comment = reply;
            issueModel.Anonymous = anonymous;
            issueModel.Disposition = dispotion;
            issueModel.TotalAgrees = myTotalAgrees;
            issueModel.TotalDisagrees = myTotalDisagrees;

            return issueModel;
        }
    }
}
