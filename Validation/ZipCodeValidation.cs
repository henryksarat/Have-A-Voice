namespace Social.Validation {
    public static class ZipCodeValidation {
        public static bool IsValid(string aZipCode) {
            int myParsedZip;
            return int.TryParse(aZipCode, out myParsedZip) && aZipCode.Length == 5;
        }
    }
}
