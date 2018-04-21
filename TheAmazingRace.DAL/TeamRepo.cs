using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class TeamRepo : BaseRepo<Team>
    {
        private TheAmazingRaceDbContext dbContext = new TheAmazingRaceDbContext();

        public TeamRepo()
        {
            this.DbContext = dbContext;
        }

        public Team GetTeamById(int? teamId)
        {
            return dbContext.Team.Where(t => t.Id == teamId).First();
        }

        public List<Team> GetAllTeams()
        {
            return dbContext.Team.ToList();
        }

        public List<Team> GetAllTeams(bool noRaceOnly)
        {
            if (noRaceOnly)
            {
                return dbContext.Team.Where(t => t.RaceEventId == null).ToList();
            }
            else
            {
                return dbContext.Team.ToList();
            }
        }

        public List<Team> GetAllTeamsWithRaceEventId(int raceEventId)
        {
            return dbContext.Team.Where(t => t.RaceEventId == raceEventId).ToList();
        }
    }
}
