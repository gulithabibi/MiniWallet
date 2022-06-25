using Microsoft.EntityFrameworkCore;
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
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly MiniWalletContext _context;


        public UserAuthRepository(MiniWalletContext context)
        {
            _context = context;
        }

        public async Task<List<UserAuth>> Find()
        {
            try
            {
                var userAuths =await _context.UserAuths.ToListAsync();

                return userAuths;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<UserAuth> FindByID(Guid id)
        {
            try
            {
                var userAuth = await _context.UserAuths.FindAsync(id);
                return userAuth;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<UserAuth> FindByToken(string token)
        {
            try
            {
                var userAuth = _context.UserAuths.Where(x=>x.Token==token).FirstOrDefault();
                return userAuth;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task<UserAuth> Create(UserAuth userAuth)
        {
            try
            {
                _context.UserAuths.Add(userAuth);
                await _context.SaveChangesAsync();

                return userAuth;
            }catch(Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }
        
        public async Task<UserAuth> Update(UserAuth userAuth)
        {
            try
            {
                _context.UserAuths.Update(userAuth);
                _context.Entry(userAuth).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return userAuth;
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
                var userAuth = await _context.UserAuths.FindAsync(id);
                if (userAuth != null)
                {
                    _context.UserAuths.Remove(userAuth);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        public async Task Delete(string token)
        {
            try
            {
                var userAuth = _context.UserAuths.Where(x=>x.Token==token).FirstOrDefault();
                if (userAuth != null)
                {
                    _context.UserAuths.Remove(userAuth);
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
