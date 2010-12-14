
using HaveAVoice.Helpers;
using HaveAVoice.Models.Services;
using System.Web.Routing;
using System.Web.Mvc;

namespace HaveAVoice.Controllers.Admin  {
    public abstract class AdminBaseController : HAVBaseController {
        protected override void Initialize(RequestContext rc) {
            base.Initialize(rc);
        }

        public AdminBaseController(IHAVBaseService baseService)
            : base(baseService) {
        }
    }
}
