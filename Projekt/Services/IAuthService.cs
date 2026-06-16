using Projekt.DTOs.AuthenticationDTOs;

namespace Projekt.Services;

public interface IAuthService
{
    public Task<TokenDTO> Login(LoginDTO loginDto);
}