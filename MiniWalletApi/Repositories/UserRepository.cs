using MiniWalletApi.Dtos;
using MiniWalletApi.Libraries;
using MiniWalletApi.Models;
using MiniWalletApi.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
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

        public bool IsUserExist(User user)
        {
            try
            {
                var u=_context.Users.Where(x => x.Username == user.Username || x.EmailAddress==user.EmailAddress).FirstOrDefault();
                if (u == null) return false;

                return true;
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
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }catch(Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<User> Update(User user)
        {
            try
            {
                _context.Users.Update(user);
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        
    }
}
