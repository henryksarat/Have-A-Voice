using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Helpers;
using HaveAVoice.Services;
using HaveAVoice.Validation;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Helpers.UserInformation;
using System.Collections;

namespace HaveAVoice.Models.View {
    public class UpdatePrivacySettingsModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            User myUser = HAVUserInformationFactory.GetUserInformation().Details;
            UpdatePrivacySettingsModel myModel = new UpdatePrivacySettingsModel();
            List<Pair<PrivacySetting, bool>> myAllPairs = new List<Pair<PrivacySetting, bool>>();
            foreach(HAVPrivacySetting mySetting in Enum.GetValues(typeof(HAVPrivacySetting))) {
                bool mySelected = BinderHelper.GetABoolean(bindingContext, mySetting.ToString());
                PrivacySetting myPrivacySetting = PrivacySetting.CreatePrivacySetting(mySetting.ToString(), 0, string.Empty, string.Empty, string.Empty);
                Pair<PrivacySetting, bool> myPair = new Pair<PrivacySetting,bool>() {
                    First = myPrivacySetting,
                    Second = mySelected
                };
                myAllPairs.Add(myPair);
            }

            myModel.PrivacySettings = myAllPairs;

            return myModel;      
        }
    }
}
