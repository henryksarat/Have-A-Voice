using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace HaveAVoice.Helpers.ActionMethods {
    public class AcceptParameterAttribute : ActionMethodSelectorAttribute {
        public string Name { get; set; }
        public string Value { get; set; }

        public override bool IsValidForRequest(ControllerContext aControllerContext, MethodInfo aMethodInfo) {
            var req = aControllerContext.RequestContext.HttpContext.Request;
            return req.Form[this.Name] == this.Value;
         }
    }
}
