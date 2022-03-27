using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Context;
using WebAPI.Models.Entity;
using WebAPI.Interfaces;
using WebAPI.Models.ViewModel;
using AutoMapper;

namespace WebAPI.Services
{
	public class UserInfoService : IUserInfoService
    {
        private readonly WebApiDbContext _context;
        IMapper iMapper;

        public UserInfoService(WebApiDbContext context) : base(context)
        {
            _context = context;
            var mapperConfig = new MapperConfiguration(userInfo => {
                userInfo.CreateMap<UserInfo, UserInfoViewModel>();
                userInfo.CreateMap<UserInfoViewModel, UserInfo>();
            });
            iMapper = mapperConfig.CreateMapper();
        }

        public override async Task<List<UserInfoViewModel>> GetUsers()
        {
            var userInfoListDb = await _context.Users.ToListAsync();
            return iMapper.Map<List<UserInfo>, List<UserInfoViewModel>>(userInfoListDb);
        }

        public override async Task<UserInfoViewModel> GetUser(int id)
        {
            var userInfoDb = await _context.Users.FindAsync(id);
            return iMapper.Map<UserInfo, UserInfoViewModel>(userInfoDb);
        }

        public override async Task<UserInfoViewModel> UpdateUser(int id, UserInfoViewModel userInfo)
        {
            if (id != userInfo.Id)
            {
                return null;
            }
            var userInfoDb = iMapper.Map<UserInfoViewModel, UserInfo>(userInfo);
            _context.Entry(userInfoDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return iMapper.Map<UserInfo, UserInfoViewModel>(userInfoDb);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public override async Task<UserInfoViewModel> CreateUser(UserInfoViewModel userInfo)
        {
            var userInfoDb = iMapper.Map<UserInfoViewModel, UserInfo>(userInfo);
            userInfoDb.CreatedAt = DateTime.UtcNow;
            userInfoDb.IsDeleted = false;
            _context.Users.Add(userInfoDb);
            await _context.SaveChangesAsync();

            userInfo = iMapper.Map<UserInfo, UserInfoViewModel>(userInfoDb);

            return userInfo;
        }

        public override async Task<UserInfoViewModel> DeleteUser(int id)
        {
            var userInfo = new UserInfoViewModel();
            if (UserExists(id))
            {
                var userInfoDb = await _context.Users.FindAsync(id);
                userInfoDb.IsDeleted = true;
                userInfoDb.DeletedAt = DateTime.UtcNow;

                _context.Entry(userInfoDb).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                userInfo = iMapper.Map<UserInfo, UserInfoViewModel>(userInfoDb);
            }
            else
			{
                return null;
			}

            return userInfo;
        }

        public override async Task<UserInfoViewModel> RetrieveUser(int id)
        {
            var userInfo = new UserInfoViewModel();
            if (UserExistsInTrash(id))
            {
                var userInfoDb = await _context.Users.FindAsync(id);
                userInfoDb.IsDeleted = false;
                userInfoDb.DeletedAt = DateTime.UtcNow;

                _context.Entry(userInfoDb).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                userInfo = iMapper.Map<UserInfo, UserInfoViewModel>(userInfoDb);

            }
            else
            {
                return null;
            }

            return userInfo;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id && !e.IsDeleted);
        }
        private bool UserExistsInTrash(int id)
        {
            return _context.Users.Any(e => e.Id == id && e.IsDeleted);
        }
    }
}
