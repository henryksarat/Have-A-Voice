using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.AdminFeatures;

namespace HaveAVoice.Models.View {
    public class RestrictionModelBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            IHAVRoleService restrictionService = new HAVRoleService(new ModelStateWrapper(null));
            List<Role> roles = restrictionService.GetAllRoles().ToList();

            string myName = BinderHelper.GetA(aBindingContext, "Name");
            string myDescription = BinderHelper.GetA(aBindingContext, "Description");
            int myRestrictionId = Convert.ToInt32(BinderHelper.GetAInt(aBindingContext, "id"));
            
            int myIssuePostsWithinTimeLimit = BinderHelper.GetAInt(aBindingContext, "IssuePostsWithinTimeLimit");
            long myIssuePostsTimeLimit = BinderHelper.GetALong(aBindingContext, "IssuePostsTimeLimit");
            long myIssuePostsWaitTimeBeforePostSeconds = BinderHelper.GetALong(aBindingContext, "issuePostsWaitTimeBeforePostSeconds");
            int myIssueReportsBeforeLockout = BinderHelper.GetAInt(aBindingContext, "IssueReportsBeforeLockout");
            
            int myIssueReplyPostsWithinTimeLimit = BinderHelper.GetAInt(aBindingContext, "IssueReplyPostsWithinTimeLimit");
            long myIssueReplyPostsTimeLimit = BinderHelper.GetALong(aBindingContext, "IssueReplyPostsTimeLimit");
            long myIssueReplyPostsWaitTimeBeforePostSeconds = BinderHelper.GetALong(aBindingContext, "IssueReplyPostsWaitTimeBeforePostSeconds");
            
            int myIssueReplyCommentPostsWithinTimeLimit = BinderHelper.GetAInt(aBindingContext, "IssueReplyCommentPostsWithinTimeLimit");
            long myIssueReplyCommentPostsTimeLimit = BinderHelper.GetALong(aBindingContext, "IssueReplyCommentPostsTimeLimit");
            long myIssueReplyCommentPostsWaitTimeBeforePostSeconds = BinderHelper.GetALong(aBindingContext, "IssueReplyCommentPostsWaitTimeBeforePostSeconds");
            
            int myMergeIssueRequestWithinTimeLimit = BinderHelper.GetAInt(aBindingContext, "MergeIssueRequestWithinTimeLimit");
            long myMergeIssueRequestTimeLimit = BinderHelper.GetALong(aBindingContext, "MergeIssueRequestTimeLimit");
            long myMergeIssueRequestWaitTimeBeforePostSeconds = BinderHelper.GetALong(aBindingContext, "MergeIssueRequestWaitTimeBeforePostSeconds");

            return new RestrictionModel.Builder(myRestrictionId)
                .Name(myName)
                .Description(myDescription)
                .IssuePostsWithinTimeLimit(myIssuePostsWithinTimeLimit)
                .IssuePostsTimeLimit(myIssuePostsTimeLimit)
                .IssuePostsWaitTimeBeforePostSeconds(myIssuePostsWaitTimeBeforePostSeconds)
                .IssueReplyCommentPostsWithinTimeLimit(myIssueReplyPostsWithinTimeLimit)
                .IssueReplyCommentPostsTimeLimit(myIssueReplyCommentPostsTimeLimit)
                .IssueReplyPostsWaitTimeBeforePostSeconds(myIssueReplyPostsWaitTimeBeforePostSeconds)
                .IssueReplyCommentPostsWithinTimeLimit(myIssueReplyCommentPostsWithinTimeLimit)
                .IssueReplyCommentPostsTimeLimit(myIssueReplyCommentPostsTimeLimit)
                .IssueReplyCommentPostsWaitTimeBeforePostSeconds(myIssueReplyCommentPostsWaitTimeBeforePostSeconds)
                .IssueReportsBeforeLockout(myIssueReportsBeforeLockout)
                .Build();
        }
    }
}
