using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityOfMe.Models;
using Social.Photo.Services;
using UniversityOfMe.Models.View;
using Social.Generic.Models;

namespace UniversityOfMe.Services.Photos {
    public interface IUofMePhotoService : IPhotoService<User, PhotoAlbum, Photo, Friend> {
        PhotoDisplayView GetPhotoDisplayView(User aUserInformation, int aPhotoId);
    }
}