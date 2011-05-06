using Social.Photo.Services;
using UniversityOfMe.Models;

namespace UniversityOfMe.Services.Photos {
    public interface IPhotoCommentService {
        bool AddCommentToPhoto(User aCommentingUser, int aPhotoId, string aComment);
    }
}