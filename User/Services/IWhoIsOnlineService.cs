namespace Social.Users.Services {
    public interface IWhoIsOnlineService<T, U> {
        void AddToWhoIsOnline(T aCurrentUser, string aCurrentIpAddress);
        bool IsOnline(T aCurrentUser, string aCurrentIpAddress);
        void RemoveFromWhoIsOnline(T currentUser, string currentIpAddress);
    }
}