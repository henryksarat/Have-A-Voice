using System.Web.Mvc;
using Social.Generic.Constants;

namespace Social.Validation {
    public class ModelStateWrapper : IValidationDictionary {
        private ModelStateDictionary theModelState;

        public ModelStateWrapper(ModelStateDictionary aModelState) {
            theModelState = aModelState;
        }

        public void AddError(string aKey, string attemptedValue, string anErrorMessage) {
            theModelState.AddModelError(aKey, anErrorMessage);
            theModelState.SetModelValue(aKey, new ValueProviderResult(attemptedValue, attemptedValue, null));
        }

        public bool isValid {
            get { return theModelState.IsValid; }
        }

        public void ForceModleStateExport() {
            theModelState.AddModelError(ValidationKeys.FORCED_MODELSTATE_EXPORT, string.Empty);
        }
    }
}
