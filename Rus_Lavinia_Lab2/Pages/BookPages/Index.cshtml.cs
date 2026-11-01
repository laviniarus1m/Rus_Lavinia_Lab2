
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;
Rus_Lavinia_Lab2.Data;

namespace Rus_Lavinia_Lab2.Pages.BookPages
{
    public class IndexModel : PageModel
    {
        private readonly Rus_Lavinia_Lab2Context _context;

        public IndexModel(
            Rus_Lavinia_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = default!;

        public BookData BookD { get; set; } = default!;

        public int BookID { get; set; }
        public int CategoryID { get; set; }

        public string TitleSort { get; set; } = string.Empty;
        public string AuthorSort { get; set; } = string.Empty;
        public string CurrentFilter { get; set; } = string.Empty;

        public async Task OnGetAsync(
            int? id,
            int? categoryID,
            string? sortOrder,
            string? searchString)
        {
            BookD = new BookData();

            TitleSort = String.IsNullOrEmpty(sortOrder)
                ? "title_desc" : "";

            AuthorSort = sortOrder == "author"
                ? "author_desc" : "author";

            CurrentFilter = searchString ?? "";

            BookD.Books = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                BookD.Books = BookD.Books.Where(s =>
                    s.Author.FirstName.Contains(searchString)
                    || s.Author.LastName.Contains(searchString)
                    || s.Title.Contains(searchString));
            }

            if (id != null)
            {
                BookID = id.Value;

                Book book = BookD.Books
                    .Where(i => i.ID == id.Value)
                    .Single();

                BookD.Categories =
                    book.BookCategories.Select(s => s.Category);
            }

            switch (sortOrder)
            {
                case "title_desc":
                    BookD.Books = BookD.Books
                        .OrderByDescending(s => s.Title);
                    break;

                case "author_desc":
                    BookD.Books = BookD.Books
                        .OrderByDescending(s => s.Author.FullName);
                    break;

                case "author":
                    BookD.Books = BookD.Books
                        .OrderBy(s => s.Author.FullName);
                    break;

                default:
                    BookD.Books = BookD.Books
                        .OrderBy(s => s.Title);
                    break;
            }

            if (categoryID != null)
            {
                CategoryID = categoryID.Value;

                var selectedBook = BookD.Books
                    .Where(i => i.ID == BookID)
                    .SingleOrDefault();

                if (selectedBook != null)
                {
                    BookD.BookCategories =
                        selectedBook.BookCategories
                        .Where(x => x.CategoryID == categoryID.Value);
                }
            }

            Book = BookD.Books.ToList();
        }
    }
}
