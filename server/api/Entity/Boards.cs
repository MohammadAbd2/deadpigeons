using efscaffold;
using efscaffold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Entity;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly MyDbContext _context;

    public BoardsController(MyDbContext context)
    {
        _context = context;
    }

    // ----------------------
    // GET: /api/boards
    // ----------------------
    [HttpGet]
    public async Task<ActionResult<List<Board>>> GetAll()
    {
        return await _context.Boards.ToListAsync();
    }

    // ----------------------
    // GET: /api/boards/{id}
    // ----------------------
    [HttpGet("{id}")]
    public async Task<ActionResult<Board>> GetById(string id)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board == null)
            return NotFound();

        return board;
    }

    // ----------------------
    // POST: /api/boards
    // ----------------------
    [HttpPost]
    public async Task<ActionResult<Board>> Create(Board board)
    {
        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = board.Id }, board);
    }

    // ----------------------
    // PUT: /api/boards/{id}
    // ----------------------
    [HttpPut("{id}")]
    public async Task<ActionResult<Board>> Update(string id, Board board)
    {
        if (id != board.Id)
            return BadRequest("ID mismatch.");

        var existingBoard = await _context.Boards.FindAsync(id);
        if (existingBoard == null)
            return NotFound();

        existingBoard.Name = board.Name;
        existingBoard.Weeknumber = board.Weeknumber;
        existingBoard.Totalwinners = board.Totalwinners;
        existingBoard.Winningnumbers = board.Winningnumbers;
        existingBoard.Winningusers = board.Winningusers;
        existingBoard.Isopen = board.Isopen;

        await _context.SaveChangesAsync();

        return existingBoard;
    }

    // ----------------------
    // DELETE: /api/boards/{id}
    // ----------------------
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(string id)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board == null)
            return NotFound();

        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();

        return id;
    }
}
