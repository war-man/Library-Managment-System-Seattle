﻿using LMS.Models;
using LMS.Models.Enums;
using LMS.Models.ModelsContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Tests.ModelsTests.HistoryRegistryTests
{
    [TestClass]
    public class Constructor_Should
    {
        private const string username = "username";
        private const string password = "password";
        private const string title = "title";
        private const string author = "author";
        private const int pages = 200;
        private const int year = 2019;
        private const string country = "Bulgaria";
        private const string language = "bulgarian";
        private const SubjectCategory subject = SubjectCategory.Action;
        private const string isbn = "978-1-940313-09-165351035";
        private const string returnDate = "14-04-2019";

        [TestMethod]
        public void MakeInstanceOfBookClass_WhenCorrectValuesPassed()
        {
            var sut = new HistoryRegistry(title, author, pages, year, country, language, subject, isbn, username, returnDate);
            Assert.IsInstanceOfType(sut, typeof(IHistoryRegistry));
        }
    }
}
