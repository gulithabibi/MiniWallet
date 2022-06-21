using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MiniWalletApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MiniWalletContext _context;


        public UserRepository(MiniWalletContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Find()
        {
            try
            {
                var users = _context.Users.ToList();

                return users;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<User> FindByID(Guid id)
        {
            try
            {
                var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<User> FindByUsernamePwd(string username, string password)
        {
            try
            {
                var user = _context.Users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == password).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<User> Create(User user)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<User> Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
