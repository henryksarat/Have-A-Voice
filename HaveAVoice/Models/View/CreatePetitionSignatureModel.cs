using System.ComponentModel.DataAnnotations;
using Social.Generic.Models;

namespace HaveAVoice.Models.View {
    public class CreatePetitionSignatureModel : BasicLocationModel {
        public int PetitionId { get; set; }
        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Address { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Comment { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }
    }
}