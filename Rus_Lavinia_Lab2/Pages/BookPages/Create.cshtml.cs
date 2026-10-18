using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BookPages;

public class CreateModel : PageModel
{
    private readonly Rus_Lavinia_Lab2Context _context;

    public CreateModel(Rus_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ViewData["AuthorsID"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
    _context.Authors.Select(a => new
    {
        a.ID,
        FullName = a.FirstName + " " + a.LastName
    }),
    "ID",
    "FullName"
);
            return Page();
        }

        _context.Book.Add(Book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
