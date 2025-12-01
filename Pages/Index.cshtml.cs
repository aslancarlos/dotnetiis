using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductApp.Data;
using ProductApp.Models;

namespace ProductApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            Products = await _context.Products
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}

