namespace Social.Site.Services {
    public interface IContactUsService {
        bool AddContactUsInquiry(string anEmail, string anInquiryType, string anInquiry);
    }
}