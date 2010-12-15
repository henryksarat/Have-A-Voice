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

namespace HaveAVoice.Models.View {
    public class ComplaintModelBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            IHAVIssueService issueService = new HAVIssueService(new ModelStateWrapper(null));

            int mySourceId = BinderHelper.GetAInt(aBindingContext, "sourceId");
            string myComplaint = BinderHelper.GetA(aBindingContext, "Complaint");
            string myComplaintType = BinderHelper.GetA(aBindingContext, "complaintType");

            ComplaintType myType = (ComplaintType)Enum.Parse(typeof(ComplaintType), myComplaintType);

            ComplaintModel.Builder myBuilder = new ComplaintModel.Builder(mySourceId, myType);
            ModelStateWrapper myModelWrapper = new ModelStateWrapper(aBindingContext.ModelState);
            ComplaintHelper.FillComplaintModelBuilder(myBuilder, new HAVUserService(), new HAVIssueService(myModelWrapper));

            myBuilder.Complaint(myComplaint);

            return myBuilder.Build();
        }
    }
}
