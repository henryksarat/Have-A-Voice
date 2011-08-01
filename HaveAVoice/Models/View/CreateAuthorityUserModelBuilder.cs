using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HaveAVoice.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using HaveAVoice.Helpers;

namespace HaveAVoice.Models.View {
    public class CreateAuthorityUserModelBuilder : CreateUserModel {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Token { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AuthorityType { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserPosition { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ExtraInfo { get; set; }

        public CreateAuthorityUserModelBuilder() {
            DateOfBirth = DateTime.UtcNow;
            States = new List<SelectListItem>();
        }

        public override User CreateNewModel() {
            User myUser = base.CreateNewModel();
            myUser.UserPositionId = UserPosition;
            return myUser;
        }
    }
}
