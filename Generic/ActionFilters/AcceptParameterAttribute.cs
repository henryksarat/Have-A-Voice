using System.Reflection;
using System.Web.Mvc;

namespace Social.Generic.ActionFilters {
    public class AcceptParameterAttribute : ActionMethodSelectorAttribute {
        public string Name { get; set; }
        public string Value { get; set; }

        public override bool IsValidForRequest(ControllerContext aControllerContext, MethodInfo aMethodInfo) {
            var req = aControllerContext.RequestContext.HttpContext.Request;
            return req.Form[this.Name] == this.Value;
         }
    }
}
