using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;

namespace TheAmazingRace.BLL
{
    public class RaceEventService : BaseService<RaceEvent>
    {
        private RaceEventRepo repo = new RaceEventRepo();
        private TeamRepo teamRepo = new TeamRepo();

        public RaceEventService()
        {
            this.Repo = repo;
        }

        public RaceEvent GetById(int? eventId)
        {
            if (eventId != null)
            {
                return repo.GetById((int)eventId);
            }
            else
            {
                return null;
            }
        }

        public bool Delete(int raceId)
        {
            var race = repo.GetById(raceId);
            var teams = teamRepo.GetAllByRaceEventId(raceId);

            foreach(var team in teams)
            {
                team.RaceEventId = null;
            }

            repo.Delete(race);

            return repo.SaveChanges();
        }
    }
}
