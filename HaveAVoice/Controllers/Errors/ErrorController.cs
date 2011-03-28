using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Services;
using Social.Error.Services;
using Social.Generic.Services;

namespace HaveAVoice.Controllers.Errors {
    public class ErrorController : HAVBaseController {
        private IErrorService<ErrorLog> theService;

        public ErrorController()
            : base(new BaseService<User>(new HAVBaseRepository())) {
            theService = new ErrorService<ErrorLog>(new EntityHAVErrorRepository());
        }

        public ErrorController(IErrorService<ErrorLog> service, IBaseService<User> baseService) 
            : base(baseService) {
            theService = service;
        }

        public ActionResult Index() {
            return View(theService.GetAllErrors().ToList<ErrorLog>());
        }
    }
}
