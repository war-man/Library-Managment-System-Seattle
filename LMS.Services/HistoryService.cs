﻿using LMS.Data;
using LMS.Models;
using LMS.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly LMSContext _context;
        private readonly IBookService _bookService;

        public HistoryService(LMSContext context,
                              IBookService bookService)
        {
            _context = context;
            _bookService = bookService;
        }
        public async Task<HistoryRegistry> CheckoutBookAsync(string bookId, string userId)
        {
            if (bookId == null)
                throw new ArgumentException("Invalid checkout parameters: book id cannot be null.");
            if (userId == null)
                throw new ArgumentException("Invalid checkout parameters: user id cannot be null.");

            var historyRegistry = new HistoryRegistry()
            {
                BookId = bookId,
                UserId = userId,
                CheckOutDate = DateTime.Now.ToShortDateString(),
                ReturnDate = DateTime.Now.AddDays(10)
            };

            if (historyRegistry == null)
                throw new ArgumentException("Invalid operation: history registry cannot be null.");
            
            _context.HistoryRegistries.Add(historyRegistry);

            var book = await _context.Books.FirstAsync(x => x.Id == bookId).ConfigureAwait(false);
            book.IsCheckedOut = true;
            //avlb. copies prop. of this book decr. with 1 in each book 
            await _context.Books.Where(b => b.Title == book.Title && b.Author.Name == book.Author.Name && b.Language == book.Language).ForEachAsync(bc => bc.Copies--).ConfigureAwait(false);

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return historyRegistry;
        }
        public async Task<IDictionary<Book, DateTime>> GetCheckOutsOfUserAsync(string userId)
        {
            var hrOfNotReturnedBooks = await GetUserHistoryOfNotReturnedBooksAsync(userId).ConfigureAwait(false);

            var currentUserBooksAndReturnDates = new Dictionary<Book, DateTime>();

            foreach (var item in hrOfNotReturnedBooks)
            {
                var book = await _bookService.FindByIdAsync(item.BookId).ConfigureAwait(false);
                var returnDate = item.ReturnDate;
                currentUserBooksAndReturnDates.Add(book, returnDate);
            }
            return currentUserBooksAndReturnDates;
        }

        private async Task<IList<HistoryRegistry>> GetUserHistoryOfNotReturnedBooksAsync(string userId)
        => await _context.HistoryRegistries
                            .Where(hr => hr.UserId == userId && hr.IsReturned == false)
                            .ToListAsync().ConfigureAwait(false);
        private async Task<HistoryRegistry> GetHistoryRegistryAsync(string bookId, string userId)
        => await _context.HistoryRegistries
                         .FirstAsync(hr => hr.BookId == bookId && hr.UserId == userId && hr.IsReturned == false).ConfigureAwait(false);
        public async Task AutoReturnAllBooksOfUser(string userId)
        {
            var hrOfUser = await GetUserHistoryOfNotReturnedBooksAsync(userId).ConfigureAwait(false);
            foreach (var history in hrOfUser)
            {
                await ReturnBookAsync(history.BookId, history.UserId).ConfigureAwait(false);
            }
        }
        public async Task ReturnBookAsync(string bookId, string userId)
        {
            if (bookId == null)
                throw new ArgumentException();

            var hr = await GetHistoryRegistryAsync(bookId, userId).ConfigureAwait(false);
            _context.HistoryRegistries.Remove(hr);

            var book = await _bookService.FindByIdAsync(bookId).ConfigureAwait(false);
            book.IsCheckedOut = false;

            await _context.Books.Where(b => b.Title == book.Title && b.Author.Name == book.Author.Name && b.Language == book.Language).ForEachAsync(bc => bc.Copies++).ConfigureAwait(false);

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        public async Task<HistoryRegistry> RenewBookAsync(string bookId, string userId)
        {
            if (bookId == null)
                throw new ArgumentException();

            var hr = await GetHistoryRegistryAsync(bookId, userId).ConfigureAwait(false);
            hr.ReturnDate = DateTime.Now.AddDays(10);

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return hr;
        }
    }
}
