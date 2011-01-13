using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class PhotosModel {
        public int UserId { get; set; }
        public string ProfilePictureURL { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
        public List<int> SelectedPhotos { get; set; }

        public PhotosModel() {
            this.ProfilePictureURL = HAVConstants.NO_PROFILE_PICTURE_URL;
            this.Photos = new List<Photo>();
            this.SelectedPhotos = new List<int>();
        }
    }
}
