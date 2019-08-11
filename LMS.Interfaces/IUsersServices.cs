﻿using LMS.Models;
using LMS.Models.ModelsContracts;
using System;
using System.Collections.Generic;

namespace LMS.Contracts
{
    public interface IUsersServices
    {
        void LoadUsersFromJson();
        IUser CheckUserCredetials(string username, string password);
        IUser CheckUsernameInUserDb(string username);
        void AddUserToDb(IUser user);
        void RemoveUserFromDb(string username);
    }
}