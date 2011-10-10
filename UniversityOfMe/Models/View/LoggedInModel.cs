using Social.Validation;
using UniversityOfMe.Helpers;
using UniversityOfMe.Services.Marketplace;
using UniversityOfMe.Services.Notifications;
using UniversityOfMe.Services.Photos;
using UniversityOfMe.Services.Users;

namespace UniversityOfMe.Models.View {
    public class LoggedInModel {
        public LeftNavigation LeftNavigation { get; set; }
        public University University { get; set; }

        public LoggedInModel(User aUser) {
            if (aUser != null) {
                INotificationService myNotificationService = new NotificationService();
                IUofMeUserService myUserService = new UofMeUserService(new ModelStateWrapper(null));
                IUofMePhotoService myPhotoService = new UofMePhotoService();
                IMarketplaceService myMarketplaceService = new MarketplaceService(new ModelStateWrapper(null));

                bool myHasProfilePicture = myPhotoService.HasProfilePhoto(aUser);

                University = UniversityHelper.GetMainUniversity(aUser);

                LeftNavigation = new LeftNavigation() {
                    User = aUser,
                    ItemTypes = myMarketplaceService.GetItemTypes(),
                    HasProfilePicture = myHasProfilePicture,
                    IsLoggedIn = true,

                };
            } else {
                LeftNavigation = new LeftNavigation() {
                    IsLoggedIn = false
                };
            }
        }
    }
}