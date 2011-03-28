using System.Web.Routing;
using HaveAVoice.Models;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Admin  {
    public abstract class AdminBaseController : HAVBaseController {
        protected override void Initialize(RequestContext rc) {
            base.Initialize(rc);
        }

        public AdminBaseController(IBaseService<User> baseService)
            : base(baseService) {
        }
    }
}
