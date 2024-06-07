using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactAPI.Controllers
{
    [Route("api/subcategories")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public SubcategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var subcategories = await _context.Subcategories.ToListAsync();
                return Ok(subcategories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving data: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var subcategory = await _context.Subcategories.FindAsync(id);
                if (subcategory == null)
                {
                    return NotFound();
                }
                return Ok(subcategory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving data: {ex.Message}");
            }
        }
    }
}
