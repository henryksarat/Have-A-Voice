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

namespace HaveAVoice.Models.View {
    public class EditPrivacySettingsModelBinder : IModelBinder {

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            EditPrivacySettingsModel myModel = new EditPrivacySettingsModel();
            foreach(HAVPrivacySetting mySetting in Enum.GetValues(typeof(HAVPrivacySetting))) {
                bool mySelected = BinderHelper.GetABoolean(bindingContext, mySetting.ToString());
                PrivacySetting myPrivacySetting = PrivacySetting.CreatePrivacySetting(mySetting.ToString(), 0, string.Empty);
                Pair<PrivacySetting, bool> myPair = new Pair<PrivacySetting,bool>() {
                    First = myPrivacySetting,
                    Second = mySelected
                };
                myModel.PrivacySettings.Add(mySetting.ToString(), myPair);
            }
            
            return myModel;      
        }
    }
}
