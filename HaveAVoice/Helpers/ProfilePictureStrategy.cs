using HaveAVoice.Models;
using HaveAVoice.Services.Helpers;
using Social.Authentication.Helpers;

namespace HaveAVoice.Helpers {
    public class ProfilePictureStrategy : IProfilePictureStrategy<User> {
        public string GetProfilePicture(User aUser) {
            return PhotoHelper.ProfilePicture(aUser);
        }
    }
}