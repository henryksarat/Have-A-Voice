using System.Linq;
using Social.Generic.Models;

namespace UniversityOfMe.Models.SocialModels {
    public class SocialPhotoAlbumModel : AbstractPhotoAlbumModel<PhotoAlbum, Photo> {
        public static SocialPhotoAlbumModel Create(PhotoAlbum anExternal) {
            if (anExternal != null) {
                return new SocialPhotoAlbumModel(anExternal);
            }
            return null;
        }

        private SocialPhotoAlbumModel(PhotoAlbum anExternal) {
            Id = anExternal.Id;
            Name = anExternal.Name;
            Description = anExternal.Description;
            CreatedByUserId = anExternal.CreatedByUserId;
            Photos = anExternal.Photos.Select(p => SocialPhotoModel.Create(p)).ToList();
            Model = anExternal;
        }
    }
}