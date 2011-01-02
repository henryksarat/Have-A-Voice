using System;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class CreateUserModelBuilderBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            String myEmail = BinderHelper.GetA(aBindingContext, "Email");
            String myUsername = BinderHelper.GetA(aBindingContext, "Username");
            String myFirstName = BinderHelper.GetA(aBindingContext, "FirstName");
            String myLastName = BinderHelper.GetA(aBindingContext, "LastName");
            String myPassword = BinderHelper.GetA(aBindingContext, "Password");
            String myCity = BinderHelper.GetA(aBindingContext, "City");
            String myState = BinderHelper.GetA(aBindingContext, "State");
            DateTime myDateOfBirth = Convert.ToDateTime(BinderHelper.GetA(aBindingContext, "DateOfBirth"));
            bool myAgreement = BinderHelper.GetA(aBindingContext, "Agreement") == "true,false" ? true : false;

            return new CreateUserModelBuilder() {
                Email = myEmail,
                Username = myUsername,
                Password = myPassword,
                City = myCity,
                FirstName = myFirstName,
                LastName = myLastName,
                DateOfBirth = myDateOfBirth,
                States = new SelectList(HAVConstants.STATES, myState),
                State = myState,
                Agreement = myAgreement
            };
        }
    }
}
