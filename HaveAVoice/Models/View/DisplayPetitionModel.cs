using System.Collections.Generic;
using System.Web.Mvc;
using Social.Generic.Constants;

namespace HaveAVoice.Models.View {
    public class DisplayPetitionModel {
        public Petition Petition { get; set; }
        
        public bool ViewSignatureDetails { get; set; }

        public IEnumerable<SelectListItem> States { get; set; }

        public DisplayPetitionModel() {
            States = new SelectList(UnitedStates.STATES);
        }
    }
}