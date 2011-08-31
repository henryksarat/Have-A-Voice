using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Social.Validation {
    public static class PhotoValidation {
        public const string INVALID_IMAGE = "Please specify a proper image file that ends in .gif, .jpg, or .jpeg.";

        public static bool IsValidImageFile(string anImageFile) {
            return !String.IsNullOrEmpty(anImageFile)
                && (anImageFile.ToUpper().EndsWith(".JPG")
                || anImageFile.ToUpper().EndsWith(".JPEG")
                || anImageFile.ToUpper().EndsWith(".PNG") 
                || anImageFile.ToUpper().EndsWith(".GIF"));
        }
    }
}