using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebAPI.Models.Common;

namespace WebAPI.Services
{
	public class RoleManagerService
    {


        public List<ControllerActions> GetAllActions()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(UserInfoService).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name

                    }).ToList();
            var list = new List<ControllerActions>();
            foreach (var item in controlleractionlist)
            {
                list.Add(new ControllerActions()
                {
                    Controller = item.Controller,
                    Action = item.Action
                });
            }
            return list;
        }

    }
}
