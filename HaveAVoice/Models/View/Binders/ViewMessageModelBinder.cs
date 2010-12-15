using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using HaveAVoice.Services;
using HaveAVoice.Models.Validation;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;

namespace HaveAVoice.Models.View {
    public class ViewMessageModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVMessageService messageService = new HAVMessageService(new ModelStateWrapper(bindingContext.ModelState));
            int messageId = Int32.Parse(BinderHelper.GetA(bindingContext, "messageId"));
            Message message = messageService.GetMessage(messageId, HAVUserInformationFactory.GetUserInformation().Details);
            string reply = BinderHelper.GetA(bindingContext, "Reply");
            ViewMessageModel model = new ViewMessageModel(message);
            model.Reply = reply;
            return model;      
        }
    }
}
