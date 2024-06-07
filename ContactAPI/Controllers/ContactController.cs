using System;
using System.Linq;
using System.Threading.Tasks;
using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactAPI.Controllers
{
    [Route("api/contacts")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly DataContext _context;

        public ContactController(DataContext context)
        {
            _context = context;
        }
      
        [HttpGet] //Zwraca listę wszystkich kontaktów (bez autentykacji)
        public async Task<IActionResult> Get()
        {
            try
            {
                var contacts = await _context.Contacts.ToListAsync();
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving data: {ex.Message}");
            }
        }
        
        [HttpGet("{id}")] //Zwraca kontakt o konkretnym id (bez autentykacji)
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound();
                }
                return Ok(contact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while retrieving data: {ex.Message}");
            }
        }
        
        [HttpPost, Authorize] //Dodaje nowy kontakt (potrzebna autentykacja, dotstępne tylko dla zalogowanego użytkownika)
        public async Task<IActionResult> Post([FromBody] Contact contact)
        {
            try
            {
                //Sprawdza czy istnieje uzytkownik o takim emailu
                var existingContact = await _context.Contacts.FirstOrDefaultAsync(c => c.email == contact.email);
                if (existingContact != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Email already exists");
                }

                //Sprawdza poprawność podkategorii w rpzypadku gdy kategoria to "służbowy"(id = 1)
                if (contact.categoryid == 1)
                {
                    var subcategoryExists = await _context.Subcategories.AnyAsync(s => s.subcategoryname == contact.subcategory);
                    if (!subcategoryExists)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Invalid subcategory");
                    }
                }

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while posting data: {ex.Message}");
            }
        }
        
        [HttpPut("{id}"), Authorize] //Aktualizuje dane istniejącego kontaktu (potrzebna autentykacja)
        public async Task<IActionResult> Put(int id, [FromBody] Contact contact)
        {
            try
            {
                if (id != contact.contactid)
                {
                    return BadRequest("ID mismatch");
                }

                //Sprawdza czy email juz istnieje z wyłączeniem aktualizowanego kontaktu
                var existingContact = await _context.Contacts.FirstOrDefaultAsync(c => c.email == contact.email && c.contactid != id);
                if (existingContact != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, "Email already exists");
                }

                //Sprawdza poprawność podkategorii w rpzypadku gdy kategoria to "służbowy"(id = 1)
                if (contact.categoryid == 1)
                {
                    var subcategoryExists = await _context.Subcategories.AnyAsync(s => s.subcategoryname == contact.subcategory);
                    if (!subcategoryExists)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, "Invalid subcategory");
                    }
                }

                _context.Entry(contact).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating data: {ex.Message}");
            }
        }

        [HttpDelete("{id}"), Authorize]        //Usuwa kontakt o danym id (potrzebna autentykacja)
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var contact = await _context.Contacts.FindAsync(id);
                if (contact == null)
                {
                    return NotFound();
                }

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting data: {ex.Message}");
            }
        }
    }
}
