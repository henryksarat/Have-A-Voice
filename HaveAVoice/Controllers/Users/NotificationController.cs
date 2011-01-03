﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HaveAVoice.Services.UserFeatures;
using HaveAVoice.Services;
using HaveAVoice.Repositories;
using HaveAVoice.Validation;
using HaveAVoice.Models;

namespace HaveAVoice.Controllers.Users {
    public class NotificationController : HAVBaseController {
        private const string NOTIFICATION_ERROR = "Error retrieving notifications.";

        private const string LIST_VIEW = "List";

        private IHAVNotificationService theNotificationService;

        public NotificationController()
            : base(new HAVBaseService(new HAVBaseRepository())) {
                theNotificationService = new HAVNotificationService();
        }

        public NotificationController(IHAVBaseService aBaseService, IHAVNotificationService aNoticiationService)
            : base(aBaseService) {
                theNotificationService = aNoticiationService;
        }

        public ActionResult List() {
            if (!IsLoggedIn()) {
                return RedirectToLogin();
            }

            IEnumerable<Board> myBoardMessages = new List<Board>();

            try {
                User myUser = GetUserInformaton();
                myBoardMessages =  theNotificationService.GetNotifications(myUser);
            } catch (Exception myException) {
                LogError(myException, NOTIFICATION_ERROR);
                ViewData["Message"] = NOTIFICATION_ERROR;
            }

            return View(LIST_VIEW, myBoardMessages);
        }
    }
}
