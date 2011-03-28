using HaveAVoice.Controllers;

namespace BaseWebsite.Controllers.Admin  {
    public abstract class AdminBaseController<T> : BaseController {
        /*
        protected override void Initialize(RequestContext rc) {
            base.Initialize(rc);
        }

        public AdminBaseController(IBaseService<T> baseService)
            : base(baseService) {
        }*/
    }
}
