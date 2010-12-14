using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HaveAVoice.Controllers.ActionFilters {
    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute {
        protected static readonly string theKey = typeof(ModelStateTempDataTransfer).FullName;
    }
}