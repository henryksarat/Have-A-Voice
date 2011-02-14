using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Helpers.Enums;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Models.View {
    public class IssueModelBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            IHAVIssueService issueService = new HAVIssueService(new ModelStateWrapper(null));
            User myUser = HAVUserInformationFactory.GetUserInformation().Details;

            int issueId = Int32.Parse(BinderHelper.GetA(aBindingContext, "IssueId"));
            int myTotalAgrees = Int32.Parse(BinderHelper.GetA(aBindingContext, "TotalAgrees"));
            int myTotalDisagrees = Int32.Parse(BinderHelper.GetA(aBindingContext, "TotalDisagrees"));

            string reply = BinderHelper.GetA(aBindingContext, "Reply");

            Issue issue = issueService.GetIssue(issueId);

            List<string> myRegisteredRoles = new List<string>();
            myRegisteredRoles.Add(Roles.OFFICIAL);
            myRegisteredRoles.Add(Roles.REGISTERED);
            IEnumerable<IssueReplyModel> registeredUserReplys = issueService.GetReplysToIssue(myUser, issue, myRegisteredRoles, PersonFilter.People);

            List<string> myOfficialRoles = new List<string>();
            myOfficialRoles.Add(Roles.OFFICIAL);
            IEnumerable<IssueReplyModel> officialUserReplys = issueService.GetReplysToIssue(myUser, issue, myOfficialRoles, PersonFilter.Politicians);
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
