using Social.Generic.Models;

namespace HaveAVoice.Models.SocialWrappers {
    public class SocialPhotoModel : AbstractPhotoModel<Photo> {
        public static SocialPhotoModel Create(Photo anExternal) {
            if (anExternal != null) {
                return new SocialPhotoModel(anExternal);
            }
            return null;
        }

        private SocialPhotoModel(Photo anExternal) {
            Id = anExternal.Id;
            UploadedByUserId = anExternal.UploadedByUserId;
            PhotoAlbumId = anExternal.PhotoAlbumId;
            ImageName = anExternal.ImageName;
            ProfilePicture = anExternal.ProfilePicture;
            DateTimeStamp = anExternal.DateTimeStamp;
            AlbumCover = anExternal.AlbumCover;
            Model = anExternal;
        }
    }
}