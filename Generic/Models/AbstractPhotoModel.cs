using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Generic.Models {
    public abstract class AbstractPhotoModel<T> : AbstractSocialModel<T> {
        public int Id { get; set; }
        public int UploadedByUserId { get; set; }
        public int PhotoAlbumId { get; set; }
        public string ImageName { get; set; }
        public bool ProfilePicture { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public bool AlbumCover { get; set; }
    }
}
