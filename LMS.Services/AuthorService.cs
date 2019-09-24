﻿using LMS.Data;
using LMS.Models;
using LMS.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly LMSContext _context;

        public AuthorService(LMSContext context)
        {
            _context = context;
        }
        private async Task<Author> AddAuthorAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return author;
        }
        private async Task<Author> FindAuthorByNameAsync(string name)
        {
            var authorToFind = await _context.Authors.FirstOrDefaultAsync(a => a.Name == name);
            return authorToFind;
        }
        public async Task<Author> ProvideAuthorAsync(string name)
        {
            if (!CheckIfAuthorExist(name))
            {
                var author = new Author { Name = name };
                await AddAuthorAsync(author);
                return author;
            }
            else
            {
                var author = await FindAuthorByNameAsync(name);
                return author;
            }
        }
        private bool CheckIfAuthorExist(string name)
            => _context.Authors.Any(a => a.Name == name);
    }
}