using efscaffold;
using efscaffold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Entity;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly MyDbContext _context;

    public UsersController(MyDbContext context)
    {
        _context = context;
    }

    // ----------------------
    // GET: /api/users
    // ----------------------
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return await _context.Users.ToListAsync();
    }

    // ----------------------
    // GET: /api/users/{id}
    // ----------------------
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();
        return user;
    }

    // ----------------------
    // POST: /api/users
    // ----------------------
    [HttpPost]
    public async Task<ActionResult<User>> Create(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    // ----------------------
    // PUT: /api/users/{id}
    // ----------------------
    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Update(string id, User user)
    {
        if (id != user.Id)
            return BadRequest("ID mismatch.");

        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
            return NotFound();

        existingUser.Name = user.Name;
        existingUser.Phone = user.Phone;
        existingUser.Email = user.Email;
        existingUser.Password = user.Password;
        existingUser.Balance = user.Balance;
        existingUser.Isactive = user.Isactive;

        await _context.SaveChangesAsync();

        return existingUser;
    }

    // ----------------------
    // DELETE: /api/users/{id}
    // ----------------------
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(string id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return id;
    }
}
