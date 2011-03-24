using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HaveAVoice.Helpers.UserInformation;
using HaveAVoice.Models.SocialWrappers;
using Social.Generic;
using Social.Generic.Helpers;
using Social.User.Models;

namespace HaveAVoice.Models.View {
    public class UpdatePrivacySettingsModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            User myUser = HAVUserInformationFactory.GetUserInformation().Details;
            UpdatePrivacySettingsModel<PrivacySetting> myModel = new UpdatePrivacySettingsModel<PrivacySetting>();
            List<Pair<AbstractPrivacySettingModel<PrivacySetting>, bool>> myAllPairs = new List<Pair<AbstractPrivacySettingModel<PrivacySetting>, bool>>();
            foreach (SocialPrivacySetting mySetting in Enum.GetValues(typeof(SocialPrivacySetting))) {
                bool mySelected = BinderHelper.GetABoolean(bindingContext, mySetting.ToString());
                PrivacySetting myPrivacySetting = PrivacySetting.CreatePrivacySetting(mySetting.ToString(), 0, string.Empty, string.Empty, string.Empty, 0);
                Pair<AbstractPrivacySettingModel<PrivacySetting>, bool> myPair = new Pair<AbstractPrivacySettingModel<PrivacySetting>, bool>() {
                    First = SocialPrivacySettingModel.Create(myPrivacySetting),
                    Second = mySelected
                };
                myAllPairs.Add(myPair);
            }

            myModel.PrivacySettings = myAllPairs;

            return myModel;      
        }
    }
}
