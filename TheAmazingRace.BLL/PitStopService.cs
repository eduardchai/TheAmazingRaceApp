using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;

namespace TheAmazingRace.BLL
{
    public class PitStopService : BaseService<PitStop>
    {
        private PitStopRepo repo = new PitStopRepo();
        private RaceEventPitStopRepo raceEventPitStopRepo = new RaceEventPitStopRepo();

        public PitStopService()
        {
            this.Repo = repo;
        }

        public PitStop GetById(int? id)
        {
            if (id != null)
            {
                return repo.GetById((int)id);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<PitStop> GetAllForRaceEvent(int raceEventId)
        {
            var pitStops = raceEventPitStopRepo.GetPitStopByRaceId(raceEventId);

            return repo.GetAll().Except(pitStops).ToList();
        }
    }
}
