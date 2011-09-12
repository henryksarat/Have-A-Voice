using System.Collections.Generic;
using System.Web.Mvc;

namespace UniversityOfMe.Models.View {
    public class FlirtModel {
        public IEnumerable<SelectListItem> Adjectives { get; set; }
        public IEnumerable<SelectListItem> DeliciousTreats { get; set; }
        public IEnumerable<SelectListItem> Animals { get; set; }
        public string Gender { get; set; }
        public int TaggedUserId { get; set; }
        public User TaggedUser { get; set; }
        public string HairColor { get; set; }
        public string Adjective { get; set; }
        public string DeliciousTreat { get; set; }
        public string Animal { get; set; }
        public string Message { get; set; }
        public string UniversityId { get; set; }
        public string Where { get; set; }
    }
}