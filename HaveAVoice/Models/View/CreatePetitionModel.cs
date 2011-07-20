using System.ComponentModel.DataAnnotations;
using Social.Generic.Models;

namespace HaveAVoice.Models.View {
    public class CreatePetitionModel : BasicLocationModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Title { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description { get; set; }

    }
}