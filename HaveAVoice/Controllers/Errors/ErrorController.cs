using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Repositories.AdminFeatures;
using Social.Error.Services;

namespace HaveAVoice.Controllers.Errors {
    public class ErrorController : HAVBaseController {
        private IErrorService<ErrorLog> theService;

        public ErrorController() {
            theService = new ErrorService<ErrorLog>(new EntityHAVErrorRepository());
        }

        public ErrorController(IErrorService<ErrorLog> service) {
            theService = service;
        }

        public ActionResult Index() {
            return View(theService.GetAllErrors().ToList<ErrorLog>());
        }
    }
}
