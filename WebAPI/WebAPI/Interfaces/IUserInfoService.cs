using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Context;
using WebAPI.Models.ViewModel;
using WebAPI.Services;

namespace WebAPI.Interfaces
{
	public abstract class IUserInfoService
    {
        public IUserInfoService(WebApiDbContext context)
        {

        }

        public abstract Task<List<UserInfoViewModel>> GetUsers();

        public abstract Task<UserInfoViewModel> GetUser(int id);

        public abstract Task<UserInfoViewModel> UpdateUser(int id, UserInfoViewModel userInfo);

        public abstract Task<UserInfoViewModel> CreateUser(UserInfoViewModel userInfo);

        public abstract Task<UserInfoViewModel> DeleteUser(int id);

        public abstract Task<UserInfoViewModel> RetrieveUser(int id);

    }
}
