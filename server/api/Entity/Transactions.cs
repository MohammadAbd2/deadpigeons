using efscaffold;
using efscaffold.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Entity;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly MyDbContext _context;

    public TransactionsController(MyDbContext context)
    {
        _context = context;
    }

    // ----------------------
    // GET: /api/transactions
    // ----------------------
    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetAll()
    {
        return await _context.Transactions.ToListAsync();
    }

    // ----------------------
    // GET: /api/transactions/{id}
    // ----------------------
    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetById(string id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
            return NotFound();

        return transaction;
    }

    // ----------------------
    // POST: /api/transactions
    // ----------------------
    [HttpPost]
    public async Task<ActionResult<Transaction>> Create(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    // ----------------------
    // PUT: /api/transactions/{id}
    // ----------------------
    [HttpPut("{id}")]
    public async Task<ActionResult<Transaction>> Update(string id, Transaction transaction)
    {
        if (id != transaction.Id)
            return BadRequest("ID mismatch.");

        var existingTransaction = await _context.Transactions.FindAsync(id);
        if (existingTransaction == null)
            return NotFound();

        existingTransaction.Username = transaction.Username;
        existingTransaction.Transactionid = transaction.Transactionid;
        existingTransaction.Status = transaction.Status;
        existingTransaction.Balance = transaction.Balance;

        await _context.SaveChangesAsync();

        return existingTransaction;
    }

    // ----------------------
    // DELETE: /api/transactions/{id}
    // ----------------------
    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(string id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
            return NotFound();

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return id;
    }
}
