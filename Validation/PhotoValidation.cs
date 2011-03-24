using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Validation {
    public static class PhotoValidation {
        public static bool IsValidImageFile(string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile)
                && (anImageFile.ToUpper().EndsWith(".JPG") || anImageFile.ToUpper().EndsWith(".JPEG") || anImageFile.ToUpper().EndsWith(".GIF"));
        }
    }
}