using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaveAVoice.Models.View {
    public class PhotoAlbumFeedModel : FeedModel{
        public IEnumerable<Photo> Photos { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public PhotoAlbumFeedModel(User aUser) : base(aUser) {
            Photos = new List<Photo>();
        }
    }
}