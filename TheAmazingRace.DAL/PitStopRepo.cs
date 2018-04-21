using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class PitStopRepo: BaseRepo<PitStop>
    {
        private TheAmazingRaceDbContext dbContext =  new TheAmazingRaceDbContext();

        public PitStopRepo()
        {
            this.DbContext = dbContext;
        }

        public PitStop GetPitStopById(int? id)
        {
            return dbContext.PitStop.Where(p => p.Id == id).First();
        }

        public List<PitStop> GetAllPitStops()
        {
            return dbContext.PitStop.Where(p => true).ToList();
        }
    }
}
