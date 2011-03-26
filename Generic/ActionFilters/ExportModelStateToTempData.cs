using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Social.Generic.ActionFilters {
    public class ExportModelStateToTempData : ModelStateTempDataTransfer {
    public override void OnActionExecuted(ActionExecutedContext aFilterContext)
	    {
	        //Only export when ModelState is not valid
	        if (!aFilterContext.Controller.ViewData.ModelState.IsValid) {
	            //Export if we are redirecting
	            if ((aFilterContext.Result is RedirectResult) || (aFilterContext.Result is RedirectToRouteResult)) {
	                aFilterContext.Controller.TempData[theKey] = aFilterContext.Controller.ViewData.ModelState;
	            }
	        }
	 
	        base.OnActionExecuted(aFilterContext);
        }
    }
}