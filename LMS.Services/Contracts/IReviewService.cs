﻿using System.Threading.Tasks;
using LMS.Models.Models;

namespace LMS.Services.Contracts
{
    public interface IReviewService
    {
        Task CreateReviewAsync(string userId, decimal grade, string description, string bookId);
        bool CheckIfUserCanReview(string userId, string bookId);
    }
}