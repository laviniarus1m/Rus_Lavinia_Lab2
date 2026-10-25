using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BookPages
{
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Rus_Lavinia_Lab2Context _context;

        public EditModel(Rus_Lavinia_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book? book = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            Book = book;

            PopulateAssignedCategoryData(_context, Book);

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "ID",
                "PublisherName",
                Book.PublisherID);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(
            int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "Book",
                b => b.Title,
                b => b.Author,
                b => b.Price,
                b => b.PublishingDate,
                b => b.PublisherID))
            {
                UpdateBookCategories(
                    _context,
                    selectedCategories,
                    bookToUpdate);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            PopulateAssignedCategoryData(
                _context,
                bookToUpdate);

            Book = bookToUpdate;

            ViewData["PublisherID"] = new SelectList(
                _context.Publisher,
                "ID",
                "PublisherName",
                Book.PublisherID);

            return Page();
        }
    }
}