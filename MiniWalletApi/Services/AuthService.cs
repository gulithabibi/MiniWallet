using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using System.Web.Http.ModelBinding;

namespace MiniWalletApi.Services
{
    public class AuthService: IAuthService
    {
        private IUserRepository _repository;
        private IUserAuthRepository _authRepository;
        private IConfiguration _config;
        public AuthService(IConfiguration config, IUserRepository repository, IUserAuthRepository authRepository)
        {
            _config = config;
            _repository = repository;
            _authRepository = authRepository;
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
                var userAuth = new UserAuth()
                {
                    Id = user.Id,
                    Name = "MiniWalletToken",
                    Token = token,
                    ExpiredAt = DateTime.Now.AddHours(12),
                    LastUsedAt = DateTime.Now
                };
                //clean history login before insert
                await _authRepository.Delete(user.Id);
                //do insert
                await _authRepository.Create(userAuth);

                var result = new Dictionary<string, string>();
                result.Add("Token", token);

                return BaseApiResponse.GetOkResponse(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

       

        public async Task<BaseApiResponse> Logout(string token)
        {
            try
            {
                await _authRepository.Delete(token);
                return BaseApiResponse.GetOkResponse("Logout has been success.");

            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseApiResponse> Register(RegisterDto registerDto)
        {
            try
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Fullname = registerDto.Fullname,
                    EmailAddress = registerDto.EmailAddress,
                    Username = registerDto.Username,
                    Password = registerDto.Password,
                    Role = registerDto.Role
                };

                //Check existing user
                if (_repository.IsUserExist(user))
                {
                    return BaseApiResponse.GetBadRequstResponse("Username or email already exist");
                }

                await _repository.Create(user);
                return BaseApiResponse.GetOkResponse(user);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
                 expires: DateTime.Now.AddHours(12),
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
