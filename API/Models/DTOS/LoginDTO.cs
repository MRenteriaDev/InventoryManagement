using System.ComponentModel.DataAnnotations;

namespace API.DTOS;

public class LoginDTO
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}
