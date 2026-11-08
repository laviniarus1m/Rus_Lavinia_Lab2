
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BorrowingPages;

public class EditModel : PageModel
{
    private readonly Rus_Lavinia_Lab2Context _context;

    public EditModel(Rus_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Borrowing Borrowing { get; set; } = default!;

    // Afisarea datelor la deschiderea paginii Edit
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

        PopulateDropdowns();

        return Page();
    }

    // Salvarea modificarilor
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return Page();
        }

        _context.Attach(Borrowing).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BorrowingExists(Borrowing.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    // Afiseaza numele membrilor si detaliile cartilor
    // in locul ID-urilor
    private void PopulateDropdowns()
    {
        var bookList = _context.Book
            .Select(b => new
            {
                b.ID,
                BookFullName = b.Title + " - " + b.Author
            })
            .ToList();

        ViewData["BookID"] = new SelectList(
            bookList,
            "ID",
            "BookFullName",
            Borrowing.BookID
        );

        ViewData["MemberID"] = new SelectList(
            _context.Member.ToList(),
            "ID",
            "FullName",
            Borrowing.MemberID
        );
    }

    // Verificarea existentei imprumutului
    private bool BorrowingExists(int id)
    {
        return _context.Borrowing.Any(e => e.ID == id);
    }
}
