using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StreletzNoticeBoard.Areas.Admin.Models;

namespace StreletzNoticeBoard.Areas.Admin.Components
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminPanelWidget()
        {
            Statistic model = new Statistic
            {
                AllUsersCount = 0,
                Admins = 0,
                Buyers = 0,
                Sellers = 0,
                Notices = 0
            };
            return PartialView("AdminPanelWidget", model);
        }

        // GET: Statistic/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Statistic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Statistic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Statistic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Statistic/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}