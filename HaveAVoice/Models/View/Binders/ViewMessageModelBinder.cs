using System;
using System.Web.Mvc;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Services.UserFeatures;
using Social.Validation;
using HaveAVoice.Repositories.UserFeatures;
using Social.Messaging.Services;

namespace HaveAVoice.Models.View {
    public class ViewMessageModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            IMessageService<User, Message, Reply> messageService = new MessageService<User, Message, Reply>(new ModelStateWrapper(bindingContext.ModelState), new EntityHAVMessageRepository());
            int messageId = Int32.Parse(BinderHelper.GetA(bindingContext, "Id"));
            Message message = messageService.GetMessage(messageId, HAVUserInformationFactory.GetUserInformation().Details);
            string reply = BinderHelper.GetA(bindingContext, "Reply");
            ViewMessageModel model = new ViewMessageModel(message);
            model.Reply = reply;
            return model;      
        }
    }
}
