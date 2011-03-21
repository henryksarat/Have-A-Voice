using System.Web;
using System.Web.Mvc;

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
