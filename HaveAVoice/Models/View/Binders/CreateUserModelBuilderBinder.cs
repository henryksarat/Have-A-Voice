using System;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using Social.Generic.Constants;

namespace HaveAVoice.Models.View {
    public class CreateUserModelBuilderBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            String myEmail = BinderHelper.GetA(aBindingContext, "Email");
            String myFirstName = BinderHelper.GetA(aBindingContext, "FirstName");
            String myLastName = BinderHelper.GetA(aBindingContext, "LastName");
            String myGender = BinderHelper.GetA(aBindingContext, "Gender");
            String myPassword = BinderHelper.GetA(aBindingContext, "Password");
            String myCity = BinderHelper.GetA(aBindingContext, "City");
            String myState = BinderHelper.GetA(aBindingContext, "State");
            DateTime myDateOfBirth = Convert.ToDateTime(BinderHelper.GetA(aBindingContext, "DateOfBirth"));
            bool myAgreement = BinderHelper.GetA(aBindingContext, "Agreement") == "true,false" ? true : false;

            return new CreateUserModelBuilder() {
                Email = myEmail,
                Password = myPassword,
                City = myCity,
                FirstName = myFirstName,
                LastName = myLastName,
                Gender = myGender,
                DateOfBirth = myDateOfBirth,
                States = new SelectList(UnitedStates.STATES, myState),
                Genders = new SelectList(Constants.GENDERS, myGender),
                State = myState,
                Agreement = myAgreement
            };
        }
    }
}
