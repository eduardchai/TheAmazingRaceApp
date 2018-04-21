using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheAmazingRace.DAL;
using TheAmazingRace.Models;

namespace TheAmazingRace.BLL
{
    public class UserService : BaseService<User>
    {
        private UserRepo repo = new UserRepo();
        private RoleRepo roleRepo = new RoleRepo();

        public UserService()
        {
            this.Repo = repo;
        }

        public User GetUserById(string userId)
        {
            return repo.GetUserById(userId);
        }

        public bool RemoveUserFromTeam(string userId)
        {
            User user = GetUserById(userId);
            user.TeamId = null;
            repo.Update(user);

            return repo.SaveChanges();
        }

        public List<User> GetAllUsersWithRole(string role)
        {
            var roleId = roleRepo.GetRoleIdByName(role);
            return repo.GetAllUsersWithRole(roleId);
        }

        public List<User> GetAllUsersWithRole(string role, string excludedUserId)
        {
            var roleId = roleRepo.GetRoleIdByName(role);
            return repo.GetAllUsersWithRole(roleId, excludedUserId);
        }

        public List<User> GetAllParticipantsWithoutTeam()
        {
            var roleId = roleRepo.GetRoleIdByName("Participant");
            return repo.GetAllUsersWithoutTeam(roleId);
        }
    }
}
