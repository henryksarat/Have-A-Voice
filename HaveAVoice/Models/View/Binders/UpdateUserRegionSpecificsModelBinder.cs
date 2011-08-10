using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Helpers.Authority;
using Social.Generic;
using Social.Generic.Helpers;

namespace HaveAVoice.Models.View {
    public class UpdateUserRegionSpecificsModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            List<Pair<AuthorityPosition, int>> myAnswers = new List<Pair<AuthorityPosition, int>>();
            foreach (AuthorityPosition myPosition in Enum.GetValues(typeof(AuthorityPosition))) {
                int myAnswer = BinderHelper.GetAInt(bindingContext, myPosition.ToString());
                Pair<AuthorityPosition, int> myPair = new Pair<AuthorityPosition, int>() {
                    First = myPosition,
                    Second = myAnswer
                };
                myAnswers.Add(myPair);
            }

            return new EditUserSpecificRegionModel() {
                Responses = myAnswers
            };      
        }
    }
}
