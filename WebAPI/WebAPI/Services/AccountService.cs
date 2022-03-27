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
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI.Services
{
	public class AccountService : IAccountService
    {
        private readonly WebApiDbContext _context;
        IMapper iMapper;

        public AccountService(WebApiDbContext context) : base(context)
        {
            _context = context;
            var mapperConfig = new MapperConfiguration(userInfo => {
                userInfo.CreateMap<RegisterViewModel, UserInfoViewModel>();
                userInfo.CreateMap<LoginViewModel, UserInfoViewModel>();
                userInfo.CreateMap<RegisterViewModel, UserInfo>();
                userInfo.CreateMap<LoginViewModel, UserInfo>();
                userInfo.CreateMap<UserInfo, RegisterViewModel>();
                userInfo.CreateMap<UserInfo, LoginViewModel>();
                userInfo.CreateMap<UserInfo, UserInfoViewModel>();
                userInfo.CreateMap<UserInfoViewModel, UserInfo>();
            });
            iMapper = mapperConfig.CreateMapper();
        }

        public override async Task<UserInfoViewModel> Register(RegisterViewModel userInfo)
        {
            var userInfoDb = iMapper.Map<RegisterViewModel, UserInfo>(userInfo);
            userInfoDb.CreatedAt = DateTime.UtcNow;
            userInfoDb.IsDeleted = false;
            _context.Users.Add(userInfoDb);
            await _context.SaveChangesAsync();

            var userInfoLogin = iMapper.Map<UserInfo, UserInfoViewModel>(userInfoDb);

            return userInfoLogin;
        }
        public override async Task<UserInfoViewModel> Login(LoginViewModel userInfo)
        {
            var userInfoDb = await UserExists(userInfo.Email, userInfo.Password);

            var userInfoLogin = iMapper.Map<UserInfo, UserInfoViewModel>(userInfoDb);

            return userInfoLogin;
        }

        private async Task<UserInfo> UserExists(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Email == email && e.Password == password && !e.IsDeleted);
        }
    }
}
