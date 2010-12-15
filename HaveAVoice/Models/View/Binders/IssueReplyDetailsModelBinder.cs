using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;

namespace HaveAVoice.Models.View {
    public class IssueReplyDetailsModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVIssueService issueService = new HAVIssueService(new ModelStateWrapper(null));

            int myReplyId = Int32.Parse(BinderHelper.GetA(bindingContext, "id"));
            string myComment = BinderHelper.GetA(bindingContext, "Comment");

            IssueReply myReply = issueService.GetIssueReply(myReplyId);
            IEnumerable<IssueReplyComment> myComments = issueService.GetIssueReplyComments(myReplyId);

            IssueReplyDetailsModel replyDetailsModel = new IssueReplyDetailsModel(myReply, myComments);
            replyDetailsModel.Comment = myComment;

            return replyDetailsModel;
        }
    }
}
