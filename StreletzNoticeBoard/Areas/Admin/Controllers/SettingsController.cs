using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Data.Models;
using DSL.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StreletzNoticeBoard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingsController : Controller
    {
        private readonly SettingsManager settingsManager;

        public SettingsController(ApplicationDbContext context)
        {
            settingsManager = new SettingsManager(context);
        }


        // GET: SettingsController/Edit/
        public ActionResult Edit()
        {
            return View(settingsManager.GetSettings().Result);
        }

        // POST: SettingsController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*IFormCollection collection*/AppSettings newSettings)
        {
            try
            {
                AppSettings settings = settingsManager.GetSettings().Result;
                settings.DescriptionLengthInList = newSettings.DescriptionLengthInList;
                settingsManager.UpdateSettings(settings);
                return RedirectToAction(nameof(Edit));
            }
            catch
            {
                return View();
            }
        }
    }
}
