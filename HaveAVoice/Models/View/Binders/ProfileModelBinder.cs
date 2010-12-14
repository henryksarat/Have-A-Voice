using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Models.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Models.Services.UserFeatures;
using System.Collections.Generic;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Models.View {
    public class ProfileModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVProfileService myProfileService = new HAVProfileService(new ModelStateWrapper(null));

            int myUserId = BinderHelper.GetAInt(bindingContext, "UserId");

            string myBoardMessage = BinderHelper.GetA(bindingContext, "BoardMessage");
            User myViewingUser = HAVUserInformationFactory.GetUserInformation().Details;

            return myProfileService.Profile(myUserId, myViewingUser).SetBoardMessage(myBoardMessage).Build();
        }
    }
}
