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

        public RaceEventService()
        {
            this.Repo = repo;
        }

        public RaceEvent getEventById(int? eventId)
        {
            return repo.getEventById(eventId);
        }

        public List<RaceEvent> getAllEvents()
        {
            return repo.getAllEvents();
        }
    }
}
