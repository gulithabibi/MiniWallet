using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniWalletApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<BaseApiResponse> Login(UserLoginDto userLogin);

        Task<BaseApiResponse> Logout(string token);

        Task<BaseApiResponse> Register(RegisterDto registerDto);
    }
}
