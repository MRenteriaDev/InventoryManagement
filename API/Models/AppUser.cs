using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class AppUser
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime Created { get; set; }
}
