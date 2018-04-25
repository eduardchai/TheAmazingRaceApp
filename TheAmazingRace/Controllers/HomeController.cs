using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheAmazingRace.BLL;

namespace TheAmazingRace.Controllers
{
    public class HomeController : Controller
    {
        private RaceEventService raceEventService = new RaceEventService();

        public ActionResult Index()
        {
            var race = raceEventService.GetMostRecentEvent();

            if (race == null)
            {
                TempData["HasLiveEvent"] = false;
            }
            else
            {
                TempData["HasLiveEvent"] = true;
            }
            return View();
        }
    }
}