﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class RaceEventRepo: BaseRepo<RaceEvent>
    {
        private TheAmazingRaceDbContext dbContext = DbContextFactory.Create();

        public RaceEvent GetById(int eventId)
        {
            try
            {
                return dbContext.RaceEvent.Where(t => t.Id == eventId).First();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RaceEvent GetMostRecentEvent()
        {
            try
            {
                return dbContext.RaceEvent.Where(r => r.EventDate < DateTime.Now).OrderBy(r => r.EventDate).First();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
