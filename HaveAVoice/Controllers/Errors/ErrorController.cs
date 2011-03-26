using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Repositories.AdminFeatures;
using HaveAVoice.Services;
using Social.Error.Services;

namespace HaveAVoice.Controllers.Errors {
    public class ErrorController : HAVBaseController {
        private IErrorService<ErrorLog> theService;

        public ErrorController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new ErrorService<ErrorLog>(new EntityHAVErrorRepository());
        }

        public ErrorController(IErrorService<ErrorLog> service, IHAVBaseService baseService) 
            : base(baseService) {
            theService = service;
        }

        public ActionResult Index() {
            return View(theService.GetAllErrors().ToList<ErrorLog>());
        }
    }
}
