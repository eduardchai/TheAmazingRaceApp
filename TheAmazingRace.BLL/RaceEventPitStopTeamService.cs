using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;

namespace TheAmazingRace.BLL
{
    public class RaceEventPitStopTeamService : BaseService<RaceEventPitStopTeam>
    {
        private RaceEventPitStopTeamRepo repo = new RaceEventPitStopTeamRepo();
        private RaceEventPitStopRepo raceEventPitStopRepo = new RaceEventPitStopRepo();
        private TeamRepo teamRepo = new TeamRepo();

        public RaceEventPitStopTeamService()
        {
            this.Repo = repo;
        }

        public List<RaceEventPitStopTeam> GetLeaderboard(int raceEventId)
        {
            return repo.GetLeaderboard(raceEventId);
        }

        public List<RaceEventPitStopTeam> GetRaceEventPitStopTeams(int raceEventId)
        {
            return repo.GetRaceEventPitStopTeams(raceEventId);
        }

        public bool CompletePitStop(int raceEventId, int pitStopId, int teamId, int currentPitStopOrder)
        {
            var data = repo.Create();
            data.RaceEventId = raceEventId;
            data.PitStopId = pitStopId;
            data.TeamId = teamId;
            data.CompletedOn = DateTime.Now;
            data.NoOfCompletedStop = currentPitStopOrder;

            var team = teamRepo.GetById(teamId);
            var pitStops = raceEventPitStopRepo.GetPitStopByRaceId(raceEventId).ToList();
            if (currentPitStopOrder == pitStops.Count)
            {
                team.DistanceToNextStop = 0;
            }

            repo.Add(data);

            return repo.SaveChanges();
        }
    }
}
