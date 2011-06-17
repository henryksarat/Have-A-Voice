using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Social.Generic;
using Social.User.Models;
using UniversityOfMe.Models.Social;
using UniversityOfMe.UserInformation;
using UniversityOfMe.Helpers;
using Social.Generic.Helpers;
using UniversityOfMe.Models.View;

namespace UniversityOfMe.Models.Binders {
    public class UpdateFeaturesModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            User myUser = UserInformationFactory.GetUserInformation().Details;
            UpdateFeaturesModel myModel = new UpdateFeaturesModel();
            List<Pair<Feature, bool>> myAllPairs = new List<Pair<Feature, bool>>();
            foreach (Features myFeature in Enum.GetValues(typeof(Features))) {
                bool mySelected = BinderHelper.GetABoolean(bindingContext, myFeature.ToString());
                Feature myFeatureModel = Feature.CreateFeature(myFeature.ToString(),string.Empty, string.Empty, 0);
                Pair<Feature, bool> myPair = new Pair<Feature, bool>() {
                    First = myFeatureModel,
                    Second = mySelected
                };
                myAllPairs.Add(myPair);
            }

            myModel.Features = myAllPairs;

            return myModel;      
        }
    }
}
