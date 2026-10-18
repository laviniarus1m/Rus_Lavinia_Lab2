using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BookPages;

public class DetailsModel : PageModel
{
    private readonly Rus_Lavinia_Lab2Context _context;
    public DetailsModel(Rus_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

    Book = await _context.Book
    .Include(b => b.Authors)
    .Include(b => b.Publisher)
    .FirstOrDefaultAsync(m => m.ID == id);
        if (Book is null)
        {
            return NotFound();
        }
        else
        {
            Book = Book;
        }

        return Page();
    }
}
