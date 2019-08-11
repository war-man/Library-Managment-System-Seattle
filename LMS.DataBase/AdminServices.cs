﻿using LMS.Contracts;
using LMS.Contracts.DataBaseContracts;
using LMS.Models;
using LMS.Models.ModelsContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly IAdminDataBase _adminDB;
        private readonly IList<User> admins = new List<User>();
        public AdminServices(IAdminDataBase adminDB)
        {
            _adminDB = adminDB;
        }
        public void LoadAdminsFromJson()
        {
            var existingAdmins = _adminDB.ReadAdmins();

            foreach (var admin in existingAdmins)
            {
                admins.Add(admin);
            }
        }
        public User CheckAdminCredentials(string username, string password)
        {
            var admin = admins.FirstOrDefault(u => u.Username == username
                                                            && u.Password == password);
            return admin;
        }
        public User CheckUsernameInAdminDb(string username)
        {
            var admin = admins.FirstOrDefault(u => u.Username == username);
            return admin;
        }
        public bool CheckIUserInAdminDb(IUser user)
        {
            var admin = admins.FirstOrDefault(u => u == user);
            if (admin != null)
                return true;
            return false;
        }
        public void RemoveAdminFromDb(string username)
        {
            var adminToBeDeleted = admins.FirstOrDefault(x => x.Username == username);
            admins.Remove(adminToBeDeleted);
            _adminDB.RemoveAdminFromJsonDb(username);
        }
    }
}