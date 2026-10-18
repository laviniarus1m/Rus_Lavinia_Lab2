using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Rus_Lavinia_Lab2.Models;


namespace Rus_Lavinia_Lab2.Pages.PublisherPages
{
    public class CreateModel : PageModel
    {
        private readonly Rus_Lavinia_Lab2Context _context;

        public CreateModel(Rus_Lavinia_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Publisher Publisher { get; set; } = new()
        {
PublisherName = string.Empty
    };

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Publisher.PublisherName))
            {
                ModelState.AddModelError(
                    "Publisher.PublisherName",
                    "Publisher name is required");
                return Page();
            }

            _context.Add(Publisher);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}