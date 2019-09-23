﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LMS.Data;
using LMS.Models;
using LMS.Models.Models;
using Microsoft.AspNetCore.Authorization;
using LMS.Web.Models;
using LMS.Services.Contracts;
using LMS.Web.Mappers.Contracts;
using LMS.Web.Mappers;

namespace LMS.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly LMSContext _context; // DA RAZKARAM CONTEXTA ot tuk !
        private readonly IBookService _bookService;
        private readonly IMapVmToDTO _mapper;

        //Scaffolded ! ! ! ! warning
        public BookController(LMSContext context, IBookService book , IMapVmToDTO mapper)
        {
            _context = context;
            _bookService = book;
            _mapper = mapper;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            var lMSContext = _context.Books.Include(b => b.Author).Include(b => b.BookRating);
            return View(await lMSContext.ToListAsync());
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var book = await _bookService.FindByIdAsync(id);
            var vm = book.MapToBookViewModel();
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [Authorize(Roles = "Librarian , Admin")]
        public IActionResult Create()
        {
            return View();
        }
        
        [Authorize(Roles = "Librarian, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var bookDto = await _mapper.MapBookVmToDTO(bookViewModel);
                var book = await _bookService.ProvideBookAsync(bookDto);
                
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["BookRatingId"] = new SelectList(_context.Set<BookRating>(), "Id", "Id", book.BookRatingId);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,AuthorId,Pages,Year,Country,Language,Copies,IsReserved,IsCheckedOut,BookRatingId,CoverImageUrl")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            ViewData["BookRatingId"] = new SelectList(_context.Set<BookRating>(), "Id", "Id", book.BookRatingId);
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookRating)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
