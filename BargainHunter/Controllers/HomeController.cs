using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BargainHunter.Helpers;
using BargainHunter.Infrastructure;
using BargainHunter.Models;
using Hangfire;

namespace BargainHunter.Controllers
{
    public class HomeController : Controller
    {
        readonly TaskHelper _taskHelper = new TaskHelper(new AmazonHelper());
        // GET: Home
        public ActionResult Index()
        {
            using (var bhe = new BargainHunterEntities())
            {
                return View(bhe.Deals.ToList());
            }
        }

        // GET: /Create/
        public ActionResult Create()
        {
            return View();
        }

        //POST: /Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([ModelBinder(typeof(DealModelBinder))] Deal deal)
        {
            ModelState.Clear();
            TryValidateModel(deal);
            if (ModelState.IsValid)
            {
                deal = TaskHelper.AddOrUpdateDeal(deal);
                TempData["DealMessage"] = String.Format(
                    "The deal for {0} has been added with an initial price of {1:c}.", deal.DealNick,
                    deal.Price);
                return RedirectToAction("Index");
            }
            return View(deal);
        }

        public ActionResult Delete(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var bhe = new BargainHunterEntities())
            {
                bhe.Deals.Remove(bhe.Deals.Find(id));
                bhe.SaveChanges();
                RecurringJob.RemoveIfExists(id);
                return RedirectToAction("Index");
            }
        }
    }
}