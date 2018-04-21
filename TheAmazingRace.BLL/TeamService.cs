using System;
using System.Collections.Generic;
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

        public TeamService()
        {
            this.Repo = repo;
        }

        public List<Team> GetAllTeams()
        {
            return repo.GetAllTeams();
        }

        public List<Team> GetAllTeamsWithoutRace()
        {
            return repo.GetAllTeams(true);
        }

        public Team GetTeamById(int? teamId)
        {
            return repo.GetTeamById(teamId);
        }

        public List<Team> GetAllTeamsWithRaceEventId(int raceEventId)
        {
            return repo.GetAllTeamsWithRaceEventId(raceEventId);
        }
    }
}
