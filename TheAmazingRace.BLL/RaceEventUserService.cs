using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;

namespace TheAmazingRace.BLL
{
    public class RaceEventUserService : BaseService<RaceEventUser>
    {
        private RaceEventUserRepo repo = new RaceEventUserRepo();

        public RaceEventUserService()
        {
            this.Repo = repo;
        }
    }
}
