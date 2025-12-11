using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("adminboardhistory", Schema = "deadpigeons")]
public class AdminBoardHistory
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = null!;

    [Column("boardid")]
    public string BoardId { get; set; } = null!;

    [Column("adminid")]
    public string AdminId { get; set; } = null!;

    [Column("iswinner")]
    public bool IsWinner { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }
}