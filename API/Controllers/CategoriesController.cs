using API.Data;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class CategoriesController : BaseApiController
{
    
    private readonly DataContext _context;
    public CategoriesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryAsync(int id)
    {
        var category = await _context.Categories.SingleOrDefaultAsync(x => x.Id == id);

        if (category != null) return Ok(category);

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Category>> GetCategoriesAsync(Category _category)
    {
        if (!ModelState.IsValid) return BadRequest();

        _context.Categories.Add(_category);
        await _context.SaveChangesAsync();

        return Ok(_category);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return BadRequest("");
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }
}
