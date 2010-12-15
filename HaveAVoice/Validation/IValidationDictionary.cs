namespace HaveAVoice.Validation
{
    public interface IValidationDictionary
    {
        void AddError(string aKey, string attemptedValue, string anErrorMessage);
        bool isValid{get;}
    }
}