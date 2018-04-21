using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class RoleRepo : BaseRepo<Role>
    {
        private TheAmazingRaceDbContext dbContext = new TheAmazingRaceDbContext();

        public RoleRepo()
        {
            this.DbContext = dbContext;
        }

        public List<Role> GetAllRoles()
        {
            return dbContext.Role.ToList();
        }

        public string GetRoleIdByName(string roleName)
        {
            return dbContext.Role.Where(r => r.Name == roleName).First().Id;
        }
    }
}
