using System;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Validation;
using HaveAVoice.Helpers;
using HaveAVoice.Services.UserFeatures;
using System.Collections.Generic;

namespace HaveAVoice.Models.View {
    public class ImageBinder : IModelBinder {
        public object BindModel(ControllerContext aControllerContext, ModelBindingContext aBindingContext) {
            HttpPostedFileBase myImageFile = null;
            foreach(string myInputTagName in aControllerContext.RequestContext.HttpContext.Request.Files) {
                 myImageFile = aControllerContext.RequestContext.HttpContext.Request.Files[myInputTagName];
            }

            return myImageFile;
        }
    }
}
