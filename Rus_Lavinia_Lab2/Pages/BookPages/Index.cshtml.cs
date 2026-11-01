using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BookPages
{
    public class IndexModel : PageModel
    {
        private readonly Rus_Lavinia_Lab2Context _context;

        public IndexModel(Rus_Lavinia_Lab2Context context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = new List<Book>();
        public BookData BookD { get; set; } = new BookData();
        public int BookID { get; set; }
        public int CategoryID { get; set; }
        public string TitleSort { get; set; } = string.Empty;
        public string AuthorSort { get; set; } = string.Empty;
        public string CurrentFilter { get; set; } = string.Empty;

        public async Task OnGetAsync(int? id, int? categoryID, string? sortOrder, string? searchString)
        {
            BookD = new BookData();
            TitleSort = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            AuthorSort = sortOrder == "author" ? "author_desc" : "author";
            CurrentFilter = searchString ?? string.Empty;

            var books = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .ToListAsync();

            IEnumerable<Book> filteredBooks = books;
            if (!string.IsNullOrEmpty(searchString))
            {
                filteredBooks = filteredBooks.Where(b =>
                    b.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            if (id.HasValue)
            {
                BookID = id.Value;
                var selectedBook = filteredBooks.FirstOrDefault(b => b.ID == id.Value);
                if (selectedBook != null)
                    BookD.Categories = selectedBook.BookCategories?.Select(bc => bc.Category)
                        .Where(c => c != null).Select(c => c!) ?? Enumerable.Empty<Category>();
            }

            if (categoryID.HasValue)
                CategoryID = categoryID.Value;

            filteredBooks = sortOrder switch
            {
                "title_desc" => filteredBooks.OrderByDescending(b => b.Title),
                "author_desc" => filteredBooks.OrderByDescending(b => b.Author),
                "author" => filteredBooks.OrderBy(b => b.Author),
                _ => filteredBooks.OrderBy(b => b.Title)
            };

            Book = filteredBooks.ToList();
            BookD.Books = Book;
        }
    }
}
