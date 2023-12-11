using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly DataContext _context;
        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProductsAsync()
        {
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var product = await _context.Products.Include(x => x.Category)
                    .SingleOrDefaultAsync(x => x.Id == id);

            if (product != null) Ok(product);

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProductAsync(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}