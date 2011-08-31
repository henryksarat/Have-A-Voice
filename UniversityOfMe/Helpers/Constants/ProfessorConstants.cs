using UniversityOfMe.Helpers.Configuration;
namespace UniversityOfMe.Helpers.Constants {
    public class ProfessorConstants {
        public static string PROFESSOR_PHOTO_PATH = "http://s3.amazonaws.com/" + SiteConfiguration.ProfessorPhotosBucket() + "/";
        public const string NO_PROFESOR_IMAGE = "no_professor_photo.jpg";
        public const int PROFESSOR_MAX_SIZE = 207;
    }
}