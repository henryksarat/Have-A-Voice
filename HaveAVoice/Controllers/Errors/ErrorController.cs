using System.Linq;
using System.Web.Mvc;
using HaveAVoice.Models;
using HaveAVoice.Repositories;
using HaveAVoice.Services;
using HaveAVoice.Services.AdminFeatures;
using Social.Validation;

namespace HaveAVoice.Controllers.Errors
{
    public class ErrorController : HAVBaseController
    {
        private IHAVErrorService theService;

        public ErrorController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
            theService = new HAVErrorService(new ModelStateWrapper(this.ModelState));
        }

        public ErrorController(IHAVErrorService service, IHAVBaseService baseService) 
            : base(baseService) {
            theService = service;
        }

        public ActionResult Index() {
            return View(theService.GetAllErrors().ToList<ErrorLog>());
        }
    }
}
