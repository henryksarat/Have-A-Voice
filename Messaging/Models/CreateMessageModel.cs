
namespace Social.Messaging.Models {
    public class CreateMessageModel<T> {
        public T SendToUser { get; set; }
        public string DefaultSubject { get; set; }
    }
}
