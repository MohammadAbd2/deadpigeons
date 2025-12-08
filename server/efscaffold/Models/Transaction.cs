using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace efscaffold.Models;

[Table("transactions", Schema = "deadpigeons")]
public partial class Transaction
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("username")]
    public string Username { get; set; } = null!;
    
    [Column("userId")]
    public int UserId { get; set; }
    
    [Column("transactionid")]
    public string Transactionid { get; set; } = null!;

    [Column("status")]
    public int Status { get; set; }

    [Column("balance")]
    public int Balance { get; set; }
    
    [Column("TransactionDate")]
    public DateTime TransactionDate { get; set; }
}
