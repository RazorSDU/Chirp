using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chirp.API.DTOs.Auth;
using Chirp.Core.Domain.Entities;
using Chirp.Core.Domain.Interfaces.Services;
using Chirp.Core.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Chirp.API.Authentication;

public sealed class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserService userService,
                      IConfiguration configuration,
                      IPasswordHasher<User> passwordHasher)
    {
        _userService = userService;
        _configuration = configuration;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponseDto> RegisterAsync(CreateUserDto createUserDto)
    {
        var existingUser = await _userService.GetUserByUsernameAsync(createUserDto.Username);
        if (existingUser != null)
        {
            throw new Exception("Username already exists.");
        }



        var tempUser = new User()
        {
            Username = createUserDto.Username
        };

        var passwordHash = _passwordHasher.HashPassword(tempUser, createUserDto.Password);

        var createUserDtoHashed = new CreateUserDto()
        {
            Username = tempUser.Username,
            Password = passwordHash
        };

        await _userService.CreateUserAsync(createUserDtoHashed);

        try
        {
            var loginDto = new LoginDto()
            {
                Username = createUserDto.Username,
            };

            var newUser = await _userService.GetUserForAuthAsync(loginDto.Username);
            if (newUser == null)
                throw new Exception("User is null after registration. Debug: " + loginDto.Username);

            string token;

            try
            {
                token = GenerateJwtToken(newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during token generation" + ex);
                throw;
            }
            return new AuthResponseDto
            {
                Token = token,
                Username = newUser.Username,
                Expires = DateTime.UtcNow.AddHours(24)
            };

        }
        catch (Exception)
        {
            throw new Exception("User created, but failed to login");
        }
    }


    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userService.GetUserForAuthAsync(loginDto.Username);
        if (user == null)
        {
            throw new Exception("Invalid credentials");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);
        if (result != PasswordVerificationResult.Success)
        {
            throw new Exception("Invalid credentials");
        }

        try
        {
            var token = GenerateJwtToken(user);
            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Expires = DateTime.UtcNow.AddHours(24)
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception during token generation" + ex);
            throw;
        }

    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}