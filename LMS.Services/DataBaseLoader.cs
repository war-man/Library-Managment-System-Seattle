﻿using LMS.Data;
using LMS.Models;
using LMS.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace LMS.Services
{
    public class DataBaseLoader : IDataBaseLoader
    {
        private readonly LMSContext _context;
        private readonly IUserServices _userServices;
        private readonly IRoleServices _roleServices;
        private readonly IJsonServices _jsonServices;
        private const string usersDirectory = @"..\..\..\..\LMS.Data\Json\Users.json";
        private const string rolesDirectory = @"..\..\..\..\LMS.Data\Json\Roles.json";
        private const string finesDirectory = @"..\..\..\..\LMS.Data\Json\RecordFines.json";
        public DataBaseLoader(LMSContext context,
                              IUserServices userServices,
                              IRoleServices roleServices,
                              IJsonServices jsonServices)
        {
            _context = context;
            _userServices = userServices;
            _roleServices = roleServices;
            _jsonServices = jsonServices;
        }
        public void SeedDataBase()
        {
            LoadRoles();
            LoadRecordFines();
            LoadUsers();
        }
        public void LoadUsers()
        {
            var users = _jsonServices.ExtractTypesFromJson<User>(usersDirectory);
            var sqlCommand = new StringBuilder();

            foreach (var user in users)
            {
                if (!_userServices.CheckIfUserExist(user.Username))
                {
                    sqlCommand.AppendLine($@"
                    
                    SELECT *  FROM dbo.Users u
                                WHERE u.Id = {user.Id}
                        BEGIN
                        INSERT INTO dbo.Users
                             (Username, Password, RoleId, RecordFinesId) 
                        VALUES ('{user.Username}', '{user.Password}','{user.RoleId}','{user.RecordFinesId}')
                    END");
                }
            }
            if (sqlCommand.Length != 0)
                _context.Database.ExecuteSqlCommand(sqlCommand.ToString());
        }
        public void LoadRoles()
        {
            var roles = _jsonServices.ExtractTypesFromJson<Role>(rolesDirectory);
            var sqlCommand = new StringBuilder();

            foreach (var role in roles)
            {
                if (!_roleServices.CheckIfRoleExist(role.Name))
                {
                    sqlCommand.AppendLine($@"
                    
                      SELECT * FROM dbo.Roles r
                             WHERE r.Id = {role.Id}
                        BEGIN
                        INSERT INTO dbo.Roles
                             (Name) 
                        VALUES ('{role.Name}')
                      END ");
                }
            }
            if (sqlCommand.Length != 0)
                _context.Database.ExecuteSqlCommand(sqlCommand.ToString());
        }
        public void LoadRecordFines()
        {
            var records = _jsonServices.ExtractTypesFromJson<RecordFines>(finesDirectory);
            var sqlCommand = new StringBuilder();

            foreach (var record in records)
            {
                sqlCommand.AppendLine($@"
                    
                      SELECT * FROM dbo.RecordFines r
                             WHERE r.Id = {record.Id}
                        BEGIN
                        INSERT INTO dbo.RecordFines
                             (FineAmount) 
                        VALUES ('{record.FineAmount}')
                      END ");
            }
            if (sqlCommand.Length != 0)
                _context.Database.ExecuteSqlCommand(sqlCommand.ToString());
        }
    }
}