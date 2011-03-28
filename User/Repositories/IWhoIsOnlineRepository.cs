using Social.User.Models;
namespace Social.User.Repositories {
    public interface IWhoIsOnlineRepository<T, U> {
        void AddToWhoIsOnline(T currentUser, string currentIpAddress);
        AbstractWhoIsOnlineModel<U> GetAbstractWhoIsOnlineEntry(T currentUser, string currentIpAddress);
        void MarkForceLogOutOfOtherUsers(T currentUser, string currentIpAddress);
        void RemoveFromWhoIsOnline(T currentUser, string currentIpAddress);
        void UpdateTimeOfWhoIsOnline(T currentUser, string currentIpAddress);
    }
}