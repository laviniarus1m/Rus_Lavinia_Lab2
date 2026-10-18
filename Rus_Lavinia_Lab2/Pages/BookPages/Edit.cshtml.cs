using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BookPages;

public class EditModel : PageModel
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
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book.FirstOrDefaultAsync(m => m.ID == id);
        if (book is null)
        {
            return NotFound();
        }
        Book = book;
        ViewData["PublisherId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
    _context.Publisher,
    "ID",
    "PublisherName",
    Book.PublisherID
);
        ViewData["AuthorsID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
    _context.Authors.Select(a => new
    {
        a.ID,
        FullName = a.FirstName + " " + a.LastName
    }),
    "ID",
    "FullName",
    Book.AuthorsID
);
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Book).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BookExists(Book.ID))
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

    private bool BookExists(int id)
    {
        return _context.Book.Any(e => e.ID == id);
    }
}
