﻿using LMS.Contracts;
using LMS.JasonDB.Contracts;
using LMS.Models;
using LMS.Models.ModelsContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMS.Services
{
    public class HistoryDataBase : IHistoryDataBase
    {
        private readonly IJson _json;
        private readonly ILoginAuthenticator _loginAuthenticator;
        private IList<IHistoryRegistry> history = new List<IHistoryRegistry>();
        public HistoryDataBase(IJson json, ILoginAuthenticator loginAuthenticator)
        {
            _json = json;
            _loginAuthenticator = loginAuthenticator;
        }
        public void AddRegistryToHistoryDb(IHistoryRegistry registry)
        {
            history.Add(registry);
            _json.AddToCheckOutHistoryJson
                (registry.Title, registry.ISBN, registry.Username, registry.ReturnDate);
        }
        public void LoadHistoryFromJson()
        {
            var existingHistory = _json.ReadCheckOutHistory();

            foreach (var registry in existingHistory)
            {
                history.Add(registry);
            }
        }
        public void CheckBooksOfCurrentUser()
        {
            var currentUsername = _loginAuthenticator.GetCurrentUserName();

            var repeat = history.Where(x => x.Username == currentUsername).ToList();
            if (repeat.Count >= 5)
                throw new ArgumentException("You have reached the maximum amount of 5 checked-out books! If u want to check-out this item, you have to return book!");
        }
    }
}
