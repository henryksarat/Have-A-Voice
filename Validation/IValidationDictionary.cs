namespace Social.Validation
{
    public interface IValidationDictionary
    {
        void ForceModleStateExport();
        void AddError(string aKey, string attemptedValue, string anErrorMessage);
        bool isValid{get;}
    }
}