using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductApp.Data;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Product.CreatedAt = DateTime.Now;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}

