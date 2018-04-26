using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheAmazingRace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TheAmazingRace.Controllers;
using TheAmazingRace.BLL;
using System.Device;

namespace TheAmazingRace.Areas.Admin.Controllers
{
    [Authorize(Roles = ("Administrator, Staff"))]
    public class RaceEventController : Controller
    {
        private UserService userService = new UserService();
        private RaceEventService raceEventService = new RaceEventService();
        private RaceEventUserService raceEventUserService = new RaceEventUserService();
        private RaceEventPitStopService raceEventPitStopService = new RaceEventPitStopService();
        private PitStopService pitStopService = new PitStopService();
        private TeamService teamService = new TeamService();
        private RaceEventPitStopTeamService raceEventPitStopTeamService = new RaceEventPitStopTeamService();

        // GET: Admin/RaceEvent
        public ActionResult Manage()
        {
            var data = raceEventService.GetAll();
            return View(data);
        }

        // GET: Admin/RaceEvent/ManageRace/5
        public ActionResult ManageRace(int? id)
        {
            if (id > 0)
            {
                var model = raceEventService.GetById(id);
                Session["RaceEventId"] = id;
                Session["RaceStarted"] = model.EventDate < DateTime.Now;
                return View(model);
            }
            else
            {
                return RedirectToAction("Manage");
            }
        }

        // GET: Admin/RaceEvent/Monitor/5
        public ActionResult Monitor(int? id)
        {
            if (id > 0)
            {
                var model = raceEventService.GetById(id);
                Session["RaceEventId"] = id;
                return View(model);
            }
            else
            {
                return RedirectToAction("Manage");
            }
        }

        // GET: Admin/RaceEvent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/RaceEvent/Create
        [HttpPost]
        public ActionResult Create(RaceEvent race)
        {
            try
            {
                race.CreatedOn = DateTime.Now;
                race.CreatedById = User.Identity.GetUserId();

                raceEventService.Add(race);

                TempData["MessageAlert"] = new Alert { CssClass = "alert-success", Title = "Success!", Message = "Race event is successfully created." };
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/RaceEvent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id > 0)
            {
                var model = raceEventService.GetById(id);

                if (model.EventDate < DateTime.Now)
                {
                    return RedirectToAction("RaceAlreadyStarted", "RaceEVent");
                }

                return View(model);
            }
            else
            {
                return RedirectToAction("Manage");
            }
        }

        // POST: Admin/RaceEvent/Edit/5
        [HttpPost]
        public ActionResult Edit(RaceEvent race)
        {
            try
            {
                if (race.EventDate < DateTime.Now)
                {
                    return RaceAlreadyStarted();
                }

                race.UpdatedById = User.Identity.GetUserId();
                race.UpdatedOn = DateTime.Now;

                raceEventService.Update(race);

                TempData["MessageAlert"] = new Alert { CssClass = "alert-success", Title = "Success!", Message = "Race event is successfully updated." };
                return View(race);
            }
            catch
            {
                return View();
            }
        }

        // POST: Admin/RaceEvent/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var model = raceEventService.GetById(id);
                if (model.EventDate < DateTime.Now)
                {
                    return RaceAlreadyStarted();
                }
                raceEventService.Delete(id);
                return RedirectToAction("Manage");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult _PitStopOrder(int raceEventId)
        {
            var models = raceEventPitStopService.GetAllByRaceEventId(raceEventId);
            return PartialView("_PitStopOrder", models);
        }

        public ActionResult _PitStopList()
        {
            var raceEventId = (int)Session["RaceEventId"];
            var models = pitStopService.GetAllForRaceEvent(raceEventId);
            return PartialView("_PitStopList", models);
        }

        public ActionResult AddPitStopToRace(int raceEventId, int pitStopId)
        {
            var model = raceEventService.GetById(raceEventId);
            if (model.EventDate < DateTime.Now)
            {
                return RaceAlreadyStarted();
            }
            raceEventPitStopService.AddPitStop(raceEventId, pitStopId);
            return RedirectToAction("ManageRace", "RaceEvent", new { id = raceEventId });
        }

        public ActionResult RemovePitStopFromRace(int raceEventId, int removedPitStopId, List<int> pitStopIds)
        {
            var model = raceEventService.GetById(raceEventId);
            if (model.EventDate < DateTime.Now)
            {
                return RaceAlreadyStarted();
            }
            raceEventPitStopService.RemovePitStop(raceEventId, removedPitStopId);
            UpdatePitStops(raceEventId, pitStopIds);
            return null;
        }

        [HttpPost]
        public ActionResult UpdatePitStops(int raceEventId, List<int> pitStopIds)
        {
            var model = raceEventService.GetById(raceEventId);
            if (model.EventDate < DateTime.Now)
            {
                return RaceAlreadyStarted();
            }
            raceEventPitStopService.UpdatePitStopOrder(raceEventId, pitStopIds);
            return null;
        }

        public ActionResult _StaffManagement(int raceEventId)
        {
            var data = raceEventUserService.GetAll();
            return PartialView("_StaffManagement", data);
        }

        public ActionResult _StaffList()
        {
            var data = userService.GetAllByRoleName("Staff");
            return PartialView("_StaffList", data);
        }

        public ActionResult AddStaffToRace(int raceEventId, string staffId)
        {
            raceEventUserService.AddStaffToRace(raceEventId, staffId);
            return _StaffManagement(raceEventId);
        }

        [HttpPost]
        public ActionResult RemoveStaffFromRace(int raceEventId, string staffId)
        {
            raceEventUserService.RemoveStaffFromRace(raceEventId, staffId);
            return _StaffManagement(raceEventId);
        }

        public ActionResult _TeamManagement(int raceEventId)
        {
            var data = teamService.GetAllByRaceEventId(raceEventId);
            return PartialView("_TeamManagement", data);
        }

        public ActionResult _TeamList()
        {
            var data = teamService.GetAllTeamsWithoutRace();
            return PartialView("_TeamList", data);
        }

        public ActionResult AddTeamToRace(int raceEventId, int teamId)
        {
            var model = raceEventService.GetById(raceEventId);
            if (model.EventDate < DateTime.Now)
            {
                return RaceAlreadyStarted();
            }

            teamService.AddTeamToRace(raceEventId, teamId, User.Identity.GetUserId());
            return _TeamManagement(raceEventId);
        }

        [HttpPost]
        public ActionResult RemoveTeamFromRace(int raceEventId, int teamId)
        {
            var model = raceEventService.GetById(raceEventId);
            if (model.EventDate < DateTime.Now)
            {
                return RaceAlreadyStarted();
            }

            teamService.RemoveTeamFromRace(raceEventId, teamId, User.Identity.GetUserId());
            return _TeamManagement(raceEventId);
        }

        public ActionResult _TeamCompletePanel(int raceEventId)
        {
            var models = raceEventPitStopTeamService.GetRaceEventPitStopTeams(raceEventId);
            return PartialView("_TeamCompletePanel", models);
        }

        [AllowAnonymous]
        public ActionResult _Leaderboard(int raceEventId)
        {
            var models = raceEventPitStopTeamService.GetLeaderboard(raceEventId);
            return PartialView("_Leaderboard", models);
        }

        [HttpPost]
        public ActionResult CompletePitStop(int raceEventId, int pitStopId, int teamId, int currentPitStopOrder)
        {
            var model = raceEventService.GetById(raceEventId);
            if (model.EventDate > DateTime.Now)
            {
                return new HttpStatusCodeResult(600, "Your action is not allowed since the race is not yet started!");
            }
            raceEventPitStopTeamService.CompletePitStop(raceEventId, pitStopId, teamId, currentPitStopOrder);
            return _TeamCompletePanel(raceEventId);
        }

        public ActionResult RaceAlreadyStarted()
        {
            return View();
        }

        public ActionResult RaceNotYetStarted()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetPitStopLocation(int raceEventId)
        {
            var data = raceEventPitStopService.GetAllByRaceEventId(raceEventId);
            var pitstops = new List<Object>();

            foreach(var r in data)
            {
                pitstops.Add(new
                {
                    name = r.PitStop.PitStopName,
                    desc = r.PitStop.ChallengeDescription,
                    address = r.PitStop.Address,
                    latitude = r.PitStop.Latitude,
                    longitude = r.PitStop.Longitude
                });
            }

            return Json(pitstops, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetStaffLocation(int raceEventId)
        {
            var data = raceEventUserService.GetEventStaffByRaceEventId(raceEventId);
            var staffs = new List<Object>();

            foreach (RaceEventUser race in data)
            {
                staffs.Add(new
                {
                    name = race.User.Name,
                    latitude = race.CurrentLat,
                    longitude = race.CurrentLong
                });
            }

            return Json(staffs, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetTeamLocation(int raceEventId)
        {
            var data = teamService.GetAllByRaceEventId(raceEventId);
            var teams = new List<Object>();
            foreach (Team t in data)
            {
                teams.Add(new
                {
                    teamId = t.Id,
                    name = t.Name,
                    latitude = t.CurrentLat,
                    longitude = t.CurrentLong
                });
            }

            return Json(teams, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult LocationProcessor(int eventId, int teamId, string staffId, double latitude, double longtitude, string type)
        {
            bool result = false;
            if (type == "TEAM")
            {
                result = teamService.UpdateTeamLocation(teamId, longtitude, latitude);
            }
            else if (type == "STAFF")
            {
                result = raceEventUserService.UpdateStaffLocation(eventId, staffId, longtitude, latitude);
            }
            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult GetPublicMap()
        {
            var race = raceEventService.GetMostRecentEvent();
            Session["RaceEventId"] = race.Id;
            return PartialView("_EventMap");
        }

        [AllowAnonymous]
        public ActionResult GetPublicLeaderboard()
        {
            var race = raceEventService.GetMostRecentEvent();
            Session["RaceEventId"] = race.Id;
            return _Leaderboard(1);
        }
    }
}
