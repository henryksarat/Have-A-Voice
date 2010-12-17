using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HaveAVoice.Models {
    public class BasicTextModelWrapper {
        public int Id { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Body { get; set; }
    }
}