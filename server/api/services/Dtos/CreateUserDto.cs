using System.ComponentModel.DataAnnotations;

namespace api.services.Dtos;

public record CreateUserDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public int Balance { get; set; }

    public bool Isactive { get; set; }
}
