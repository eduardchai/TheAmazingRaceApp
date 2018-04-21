using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class RaceEventRepo: BaseRepo<RaceEvent>
    {
        private TheAmazingRaceDbContext dbContext = new TheAmazingRaceDbContext();

        public RaceEventRepo()
        {
            this.DbContext = dbContext;
        }

        public RaceEvent getEventById(int? eventId)
        {
            return dbContext.RaceEvent.Where(t => t.Id == eventId).First();
        }

        public List<RaceEvent> getAllEvents()
        {
            return dbContext.RaceEvent.ToList();
        }
    }
}
