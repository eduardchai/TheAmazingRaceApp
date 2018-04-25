using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;

namespace TheAmazingRace.BLL
{
    public class TeamService : BaseService<Team>
    {
        private TeamRepo repo = new TeamRepo();
        private UserRepo userRepo = new UserRepo();
        private RaceEventPitStopRepo raceEventPitStopRepo = new RaceEventPitStopRepo();
        private RaceEventPitStopTeamRepo raceEventPitStopTeamRepo = new RaceEventPitStopTeamRepo();

        public TeamService()
        {
            this.Repo = repo;
        }

        public IEnumerable<Team> GetAllTeamsWithoutRace()
        {
            return repo.GetAll(true);
        }

        public Team GetById(int? teamId)
        {
            if(teamId != null)
            {
                return repo.GetById((int)teamId);
            }
            else
            {
                return null;
            }
        }

        public bool Delete(int teamId)
        {
            var team = repo.GetById(teamId);
            var users = userRepo.GetAllByTeamId(teamId);

            foreach (var user in users)
            {
                user.TeamId = null;
            }

            repo.Delete(team);

            return repo.SaveChanges();
        }

        public IEnumerable<Team> GetAllByRaceEventId(int raceEventId)
        {
            return repo.GetAllByRaceEventId(raceEventId);
        }

        public bool AddTeamToRace(int raceEventId, int teamId, string updateById)
        {
            var team = GetById(teamId);
            team.RaceEventId = raceEventId;
            team.UpdatedOn = DateTime.Now;
            team.UpdatedById = updateById;

            repo.Update(team);

            return repo.SaveChanges();
        }

        public bool RemoveTeamFromRace(int raceEventId, int teamId, string updateById)
        {
            var team = GetById(teamId);
            team.RaceEventId = null;
            team.UpdatedOn = DateTime.Now;
            team.UpdatedById = updateById;

            repo.Update(team);

            return repo.SaveChanges();
        }

        public bool UpdateTeamLocation(int teamId, double currLong, double currLat)
        {
            var team = GetById(teamId);
            team.CurrentLong = currLong;
            team.CurrentLat = currLat;

            var noOfCompletedPit = raceEventPitStopTeamRepo.GetNoOfCompletedPit((int)team.RaceEventId, team.Id);
            var pitStops = raceEventPitStopRepo.GetPitStopByRaceId((int)team.RaceEventId).ToList();

            if (noOfCompletedPit < pitStops.Count)
            {
                var currPitStop = pitStops[noOfCompletedPit];

                var pitStopGeoLoc = new GeoCoordinate(currPitStop.Latitude, currPitStop.Longitude);
                var teamGeoLoc = new GeoCoordinate(team.CurrentLat, team.CurrentLong);

                double distance = pitStopGeoLoc.GetDistanceTo(teamGeoLoc);

                team.DistanceToNextStop = Math.Round(distance / 1000, 3);
            } else
            {
                team.DistanceToNextStop = 0;
            }

            repo.Update(team);

            return repo.SaveChanges();
        }
    }
}
