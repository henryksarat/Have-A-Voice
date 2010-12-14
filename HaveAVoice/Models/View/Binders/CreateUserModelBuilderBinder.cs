using System;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class CreateUserModelBuilderBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            String aEmail = BinderHelper.GetA(aBindingContext, "Email");
            String aUsername = BinderHelper.GetA(aBindingContext, "Username");
            String aFullName = BinderHelper.GetA(aBindingContext, "FullName");
            String aPassword = BinderHelper.GetA(aBindingContext, "Password");
            String aCity = BinderHelper.GetA(aBindingContext, "City");
            String aState = BinderHelper.GetA(aBindingContext, "State");
            DateTime aDateOfBirth = Convert.ToDateTime(BinderHelper.GetA(aBindingContext, "DateOfBirth"));
            bool aAgreement = BinderHelper.GetA(aBindingContext, "Agreement") == "true,false" ? true : false;

            return new CreateUserModelBuilder()
                .Email(aEmail)
                .Username(aUsername)
                .Password(aPassword)
                .City(aCity)
                .FullName(aFullName)
                .DateOfBirth(aDateOfBirth)
                .States(new SelectList(HAVConstants.STATES, aState))
                .State(aState)
                .Agreement(aAgreement);
        }
    }
}
