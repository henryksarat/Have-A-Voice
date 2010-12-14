using System.Web.Mvc;

namespace HaveAVoice.Models.Validation
{
    public class ModelStateWrapper : IValidationDictionary
    {
        private ModelStateDictionary theModelState;

        public ModelStateWrapper(ModelStateDictionary aModelState)
        {
            theModelState = aModelState;
        }

        public void AddError(string aKey, string attemptedValue, string anErrorMessage)
        {
            theModelState.AddModelError(aKey, anErrorMessage);
            theModelState.SetModelValue(aKey, new ValueProviderResult(attemptedValue, attemptedValue, null));
        }

        public bool isValid
        {
            get { return theModelState.IsValid; }
        }
    }
}
