using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chirp.API.DTOs.Auth;
using Chirp.Core.Domain.Entities;
using Chirp.Core.DTOs;

namespace Chirp.Core.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(CreateUserDto createUserDto);
}
