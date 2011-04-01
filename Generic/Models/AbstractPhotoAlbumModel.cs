using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Social.Generic.Models {
    public abstract class AbstractPhotoAlbumModel<T, U> : AbstractSocialModel<T> {
        public int Id { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }

        public IEnumerable<AbstractPhotoModel<U>> Photos { get; set; }
    }
}
