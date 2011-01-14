﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;

namespace HaveAVoice.Services.UserFeatures {
    public interface IHAVPhotoAlbumService {
        bool CreatePhotoAlbum(User aUser, string aName, string aDescription);
        IEnumerable<PhotoAlbum> GetPhotoAlbumsForUser(User aUser);
    }
}