using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.AdminFeatures;
using Social.Validation;
using Social.Error.Services;
using HaveAVoice.Repositories.AdminFeatures;

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
