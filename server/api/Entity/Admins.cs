using efscaffold;
using efscaffold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Entity;

[ApiController]
[Route("api/[controller]")]
public class AdminsController : ControllerBase
{
    private readonly MyDbContext _context;

    public AdminsController(MyDbContext context)
    {
        _context = context;
    }

    // ----------------------
    // GET: /api/admins
    // ----------------------
    [HttpGet]
    public async Task<ActionResult<List<Admin>>> GetAll()
    {
        return await _context.Admins.ToListAsync();
    }

    // ----------------------
    // GET: /api/admins/{id}
    // ----------------------
    [HttpGet("{id}")]
    public async Task<ActionResult<Admin>> GetById(string id)
    {
        var admin = await _context.Admins.FindAsync(id);
        if (admin == null)
            return NotFound();

        return admin;
    }

    // ----------------------
    // POST: /api/admins
    // ----------------------
    [HttpPost]
    public async Task<ActionResult<Admin>> Create(Admin admin)
    {
        _context.Admins.Add(admin);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = admin.Id }, admin);
    }

    // ----------------------
    // PUT: /api/admins/{id}
    // ----------------------
    [HttpPut("{id}")]
    public async Task<ActionResult<Admin>> Update(string id, Admin admin)
    {
        if (id != admin.Id)
            return BadRequest("ID mismatch.");

        var existingAdmin = await _context.Admins.FindAsync(id);
        if (existingAdmin == null)
            return NotFound();

        existingAdmin.Name = admin.Name;
        existingAdmin.Email = admin.Email;
        existingAdmin.Password = admin.Password;

        await _context.SaveChangesAsync();

        return existingAdmin;
    }

    // ----------------------
    // DELETE: /api/admins/{id}
    // ----------------------
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(string id)
    {
        var admin = await _context.Admins.FindAsync(id);
        if (admin == null)
            return NotFound();

        _context.Admins.Remove(admin);
        await _context.SaveChangesAsync();

        return id;
    }
}
