﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheAmazingRace.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TheAmazingRace.Controllers;
using TheAmazingRace.BLL;

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
                var model = raceEventService.GetById(id); ;
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
                race.UpdatedById = User.Identity.GetUserId();
                race.UpdatedOn = DateTime.Now;

                raceEventService.Update(race);

                TempData["MessageAlert"] = new Alert { CssClass = "alert-success", Title = "Success!", Message = "Race event is successfully updated." };
                return View();
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
            var models = pitStopService.GetAll();
            return PartialView("_PitStopList", models);
        }

        public ActionResult AddPitStopToRace(int raceEventId, int pitStopId)
        {
            raceEventPitStopService.AddPitStop(raceEventId, pitStopId);
            return RedirectToAction("ManageRace", "RaceEvent", new { id = raceEventId });
        }

        public ActionResult RemovePitStopFromRace(int raceEventId, int removedPitStopId, List<int> pitStopIds)
        {
            raceEventPitStopService.RemovePitStop(raceEventId, removedPitStopId);
            UpdatePitStops(raceEventId, pitStopIds);
            return null;
        }

        [HttpPost]
        public ActionResult UpdatePitStops(int raceEventId, List<int> pitStopIds)
        {
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
            teamService.AddTeamToRace(raceEventId, teamId, User.Identity.GetUserId());
            return _TeamManagement(raceEventId);
        }

        [HttpPost]
        public ActionResult RemoveTeamFromRace(int raceEventId, int teamId)
        {
            teamService.RemoveTeamFromRace(raceEventId, teamId, User.Identity.GetUserId());
            return _TeamManagement(raceEventId);
        }

        public ActionResult _TeamCompletePanel(int raceEventId)
        {
            var models = raceEventPitStopTeamService.GetRaceEventPitStopTeams(raceEventId);
            return PartialView("_TeamCompletePanel", models);
        }

        public ActionResult _Leaderboard(int raceEventId)
        {
            var models = raceEventPitStopTeamService.GetLeaderboard(raceEventId);
            return PartialView("_Leaderboard", models);
        }

        [HttpPost]
        public ActionResult CompletePitStop(int raceEventId, int pitStopId, int teamId, int currentPitStopOrder)
        {
            raceEventPitStopTeamService.CompletePitStop(raceEventId, pitStopId, teamId, currentPitStopOrder);
            return _TeamCompletePanel(raceEventId);
        }
    }
}
