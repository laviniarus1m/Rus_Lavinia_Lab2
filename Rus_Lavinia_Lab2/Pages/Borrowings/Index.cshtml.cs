using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Rus_Lavinia_Lab2.Models;

namespace Rus_Lavinia_Lab2.Pages.BorrowingPages;

public class IndexModel : PageModel
{
    private readonly Rus_Lavinia_Lab2Context _context;

    public IndexModel(Rus_Lavinia_Lab2Context context)
    {
        _context = context;
    }

    public IList<Borrowing> Borrowing { get; set; } = default!;


    public async Task OnGetAsync()
    {
        Borrowing = await _context.Borrowing
            .Include(b => b.Book)
            .Include(b => b.Member)
            .ToListAsync();
    }

}
