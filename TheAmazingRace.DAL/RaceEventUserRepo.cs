using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class RaceEventUserRepo : BaseRepo<RaceEventUser>
    {
        private TheAmazingRaceDbContext dbContext = new TheAmazingRaceDbContext();

        public RaceEventUserRepo()
        {
            this.DbContext = dbContext;
        }
    }
}
