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

        public PitStopService()
        {
            this.Repo = repo;
        }

        public PitStop GetPitStopById(int? id)
        {
            return repo.GetPitStopById(id);
        }

        public List<PitStop> GetAllPitStops()
        {
            return repo.GetAllPitStops();
        }
    }
}
