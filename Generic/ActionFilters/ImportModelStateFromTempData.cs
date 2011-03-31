using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Generic.Constants;

namespace Social.Generic.ActionFilters {
    public class ImportModelStateFromTempData : ModelStateTempDataTransfer {
	    public override void OnActionExecuted(ActionExecutedContext aFilterContext) {
	        ModelStateDictionary myModelState = aFilterContext.Controller.TempData[theKey] as ModelStateDictionary;
	 
	        if (myModelState != null) {
                myModelState.Remove(Social.Generic.Constants.ValidationKeys.FORCED_MODELSTATE_EXPORT);
	            //Only Import if we are viewing
	            if (aFilterContext.Result is ViewResult) {
	                aFilterContext.Controller.ViewData.ModelState.Merge(myModelState);
	            } else {
	                //Otherwise remove it.
	                aFilterContext.Controller.TempData.Remove(theKey);
                }
	        }
	 
	        base.OnActionExecuted(aFilterContext);
	    }
    }
}