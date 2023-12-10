using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOS;
using API.Interfaces;
using API.Models;
using API.Models.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    public AccountController(DataContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")] //POST:/api/register
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO register)
    {
        using var hmac = new HMACSHA512();
        if (await UserExists(register.UserName)) return BadRequest("username is taken");

        var user = new AppUser
        {
            UserName = register.UserName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
            PasswordSalt = hmac.Key,
            Created = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDTO
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(x => x.UserName == login.UserName);

        if (user == null) return NotFound();

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized();
        }

        return new UserDTO
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
