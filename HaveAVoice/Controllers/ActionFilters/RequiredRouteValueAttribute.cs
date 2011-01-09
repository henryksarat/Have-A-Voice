using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace HaveAVoice.Controllers.ActionFilters {
    public class RequiredRouteValueAttribute {
        public class RequireRouteValuesAttribute : ActionMethodSelectorAttribute {
            public RequireRouteValuesAttribute(string[] valueNames) {
                ValueNames = valueNames;
            }

            public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo) {
                bool contains = false;

                if (ValueNames.Count<string>() == 0 && controllerContext.RequestContext.RouteData.Values.Count == 2) {
                    contains = true;
                }

                foreach (var value in ValueNames) {
                    contains = controllerContext.RequestContext.RouteData.Values.ContainsKey(value);
                    if (!contains) break;
                }
                return contains;
            }

            public string[] ValueNames { get; private set; }
        }
    }
}