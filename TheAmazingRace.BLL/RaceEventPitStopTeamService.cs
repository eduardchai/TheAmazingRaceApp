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
            var data = new RaceEventPitStopTeam
            {
                RaceEventId = raceEventId,
                PitStopId = pitStopId,
                TeamId = teamId,
                CompletedOn = DateTime.Now,
                NoOfCompletedStop = currentPitStopOrder
            };

            repo.Add(data);

            return repo.SaveChanges();
        }
    }
}
