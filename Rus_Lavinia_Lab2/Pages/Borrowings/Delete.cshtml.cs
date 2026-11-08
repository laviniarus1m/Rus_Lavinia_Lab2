
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BorrowingPages;

public class DeleteModel : PageModel
{
    private readonly Rus_Lavinia_Lab2Context _context;

    public DeleteModel(Rus_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    // Afiseaza detaliile imprumutului
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing
            .Include(b => b.Book)
            .Include(b => b.Member)
            .FirstOrDefaultAsync(m => m.ID == id);

        if (borrowing is null)
        {
            return NotFound();
        }

        Borrowing = borrowing;

        return Page();
    }

    // Sterge imprumutul din baza de date
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var borrowing = await _context.Borrowing
            .FindAsync(id);

        if (borrowing is not null)
        {
            Borrowing = borrowing;

            _context.Borrowing.Remove(Borrowing);

            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}
