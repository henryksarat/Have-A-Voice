using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Photo.Repositories;
using UniversityOfMe.Models;

namespace UniversityOfMe.Repositories.Photos {
    public interface IUofMePhotoRepository : IPhotoRepository<User, Photo> {
        void AddPhotoComment(User aCommentingUser, int aPhotoId, string aComment);
        IEnumerable<Photo> GetOtherPhotosInPhotosAlbum(Photo aPhoto);
        bool HasAlbumCoverAlready(int anAlbumId);
    }
}