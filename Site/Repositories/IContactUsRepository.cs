namespace Social.Site.Repositories {
    public interface IContactUsRepository {
        void AddContactUserInquiry(string anEmail, string anInquiryType, string aInquiry);
    }
}