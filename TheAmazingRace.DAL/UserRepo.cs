using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheAmazingRace.Models;

namespace TheAmazingRace.DAL
{
    public class UserRepo : BaseRepo<User>
    {
        private TheAmazingRaceDbContext dbContext = new TheAmazingRaceDbContext();

        public UserRepo()
        {
            this.DbContext = dbContext;
        }

        public User GetUserById(string id)
        {
            return dbContext.Users.Where(u => u.Id == id).First();
        }

        public List<User> GetAllUsers()
        {
            return dbContext.Users.ToList();
        }

        public List<User> GetAllUsers(string excludedUserId)
        {
            return dbContext.Users.Where(u => u.Id != excludedUserId).ToList();
        }

        public List<User> GetAllUsersWithRole(string roleId)
        {
            return dbContext.Users.Where(m => m.Roles.Select(r => r.RoleId).Contains(roleId)).ToList();
        }

        public List<User> GetAllUsersWithRole(string roleId, string excludedUserId)
        {
            return dbContext.Users.Where(m => m.Id != excludedUserId && m.Roles.Select(r => r.RoleId).Contains(roleId)).ToList();
        }

        public List<User> GetAllUsersWithoutTeam(string roleId)
        {
            return dbContext.Users.Where(m => m.TeamId == null && m.Roles.Select(r => r.RoleId).Contains(roleId)).ToList();
        }
    }
}
