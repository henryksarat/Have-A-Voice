namespace Social.Validation {
    public static class RangeValidation {
        public static bool IsWithinRange(int aValue, int aLower, int anUpper) {
            return aValue >= aLower && aValue <= anUpper;
        }
    }
}
