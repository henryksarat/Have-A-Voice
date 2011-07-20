namespace Social.Validation {
    public static class PhoneValidation {
        public static bool IsValid(string aPhoneNumber) {
            aPhoneNumber = aPhoneNumber.Replace('-', ' ').Replace('(', ' ').Replace(')', ' ').Trim();
            int myParsedZip;
            return int.TryParse(aPhoneNumber, out myParsedZip) && aPhoneNumber.Length == 10;
        }
    }
}
