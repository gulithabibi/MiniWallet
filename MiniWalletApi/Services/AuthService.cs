using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using MiniWalletApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MiniWalletApi.Services
{
    public class AuthService: IAuthService
    {
        private IUserRepository _repository;
        private IConfiguration _config;
        public AuthService(IConfiguration config, IUserRepository repository)
        {
            _config = config;
            _repository = repository;
        }

        public async Task<BaseApiResponse> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = Autenticate(userLogin);
                if (user == null)
                    return BaseApiResponse.GetNotFoundResponse("User not found");

                var token = Generate(user);
                //Insert to user auth


                return BaseApiResponse.GetOkResponse(token);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<BaseApiResponse> Logout()
        {
            throw new NotImplementedException();
        }

        public Task<BaseApiResponse> Register()
        {
            throw new NotImplementedException();
        }

        private string Generate(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Role, user.Role)
                };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                 _config["Jwt:Audience"],
                 claims,
                 expires: DateTime.Now.AddMinutes(15),
                 signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private User Autenticate(UserLoginDto userLogin)
        {
            try
            {
                var currentUser = _repository.FindByUsernamePwd(userLogin.Username, userLogin.Password).Result;
                if (currentUser != null)
                {
                    return currentUser;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



     
    }
}
