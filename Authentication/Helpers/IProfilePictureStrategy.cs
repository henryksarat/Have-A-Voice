﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Authentication.Helpers {
    public interface IProfilePictureStrategy<T> {
        string GetProfilePicture(T aUser);
    }
}
