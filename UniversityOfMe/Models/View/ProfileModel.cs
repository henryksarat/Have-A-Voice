using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityOfMe.Models.View {
    public class ProfileModel {
        public User User { get; set; }
        public IEnumerable<Board> Boards { get; set; }
        public int BoardCount { get; set; }
        public bool ShowAllBoards { get; set; }
        public IEnumerable<UserStatus> UserStatuses { get; set; }
        public bool HasUserStatuses {
            get {
                return UserStatuses.Count<UserStatus>() > 0;
            }
        }
        public IEnumerable<PhotoAlbum> PhotoAlbums { get; set; }
        public int PhotoAlbumCount { get; set; }
        public bool ShowAllPhotoAlbums { get; set; }
    }
}