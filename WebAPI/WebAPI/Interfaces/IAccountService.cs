using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Context;
using WebAPI.Models.ViewModel;
using WebAPI.Services;

namespace WebAPI.Interfaces
{
	public abstract class IAccountService
    {
        public IAccountService(WebApiDbContext context)
        {

        }

        public abstract Task<UserInfoViewModel> Register(RegisterViewModel userInfo);
        public abstract Task<UserInfoViewModel> Login(LoginViewModel userInfo);
    }
}
