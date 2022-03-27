using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.ViewModel;
using WebAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private IUserInfoService _service;

        public UserInfoController(IUserInfoService service)
        {
            _service = service;
        }

        // GET: api/GetUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetUsers()
        {
            return await _service.GetUsers();
        }

        // GET: api/GetUser/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoViewModel>> GetUser(int id)
        {
            var userInfo = await _service.GetUser(id);

            if (userInfo == null)
            {
                return NotFound();
            }

            return userInfo;
        }

        // PUT: api/UpdateUser/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfoViewModel>> UpdateUser(int id, UserInfoViewModel userInfo)
        {
            var res = userInfo;
            if (id != userInfo.Id)
            {
                return BadRequest();
            }

            try
            {
                res = await _service.UpdateUser(id, userInfo);
            }
            catch (Exception e)
            {
                throw e;
            }

            return res;
        }

        // POST: api/CreateUser
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserInfoViewModel>> CreateUser(UserInfoViewModel userInfo)
        {
            var res = await _service.CreateUser(userInfo);

            return res;
        }

        // DELETE: api/DeleteUser/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserInfoViewModel>> DeleteUser(int id)
        {
            var userInfo = await _service.DeleteUser(id);

            return userInfo;
        }

        // Retrieve: api/RetrieveUser/5
        [HttpPut("RetrieveUser/{id}")]
        public async Task<ActionResult<UserInfoViewModel>> RetrieveUser(int id)
        {
            var userInfo = await _service.RetrieveUser(id);

            return userInfo;
        }
    }
}
