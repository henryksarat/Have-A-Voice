using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Social.Generic.Constants;
using Social.Generic;
using System.Linq;
using HaveAVoice.Helpers.Authority;

namespace HaveAVoice.Models.View {
    public class EditUserSpecificRegionModel {
        public IEnumerable<Pair<UserPosition, SelectList>> RegionClashes { get; set; }
        public IEnumerable<Pair<AuthorityPosition, int>> Responses { get; set; }

        public bool HasRegionClash {
            get {
                return RegionClashes.Count() > 0;
            }
        }

        public EditUserSpecificRegionModel() { 
            RegionClashes = new List<Pair<UserPosition, SelectList>>();
            Responses = new List<Pair<AuthorityPosition, int>>();
        }
    }
}
