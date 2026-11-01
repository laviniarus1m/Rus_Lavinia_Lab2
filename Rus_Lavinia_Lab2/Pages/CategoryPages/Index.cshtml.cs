using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Rus_Lavinia_Lab2Context _context;

    public IndexModel(Rus_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = new List<Category>();
    public int? CategoryID { get; set; }
    public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();

    public async Task OnGetAsync(int? id)
    {
        CategoryID = id;
        Category = await _context.Category
            .AsNoTracking()
            .OrderBy(c => c.CategoryName)
            .ToListAsync();

        if (id.HasValue)
        {
            Books = await _context.BookCategory
                .AsNoTracking()
                .Where(bc => bc.CategoryID == id.Value)
                .Include(bc => bc.Book)
                    .ThenInclude(b => b.Authors)
                .Select(bc => bc.Book)
                .OrderBy(b => b.Title)
                .ToListAsync();
        }
    }
}
