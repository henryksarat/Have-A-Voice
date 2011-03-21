using System;
using System.Web.Mvc;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.UserFeatures;
using Social.Validation;

namespace HaveAVoice.Models.View {
    public class ViewMessageModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IHAVMessageService messageService = new HAVMessageService(new ModelStateWrapper(bindingContext.ModelState));
            int messageId = Int32.Parse(BinderHelper.GetA(bindingContext, "Id"));
            Message message = messageService.GetMessage(messageId, HAVUserInformationFactory.GetUserInformation().Details);
            string reply = BinderHelper.GetA(bindingContext, "Reply");
            ViewMessageModel model = new ViewMessageModel(message);
            model.Reply = reply;
            return model;      
        }
    }
}
