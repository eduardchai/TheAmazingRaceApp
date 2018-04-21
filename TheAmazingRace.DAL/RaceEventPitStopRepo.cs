using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class RaceEventPitStopRepo : BaseRepo<RaceEventPitStop>
    {
        private TheAmazingRaceDbContext dbContext = new TheAmazingRaceDbContext();

        public RaceEventPitStopRepo()
        {
            this.DbContext = dbContext;
        }

        public RaceEventPitStop GetRaceEventPitStop(int raceEventId, int pitStopId)
        {
            return dbContext.RaceEventPitStop.Where(r => r.RaceEventId == raceEventId && r.PitStopId == pitStopId).First();
        }

        public List<RaceEventPitStop> GetAllByRaceEventId(int raceId)
        {
            return dbContext.RaceEventPitStop.Where(r => r.RaceEventId == raceId).OrderBy(r => r.Order).ToList();
        }

        public RaceEventPitStop GetRaceEventPitStopByPitId(int pitId)
        {
            return dbContext.RaceEventPitStop.Where(r => r.PitStopId == pitId).First();
        }
    }
}
