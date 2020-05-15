using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AngularApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AngularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ShoppingDbContext _context;
        private readonly UserManager<Users> usermanager;
        private readonly SignInManager<Users> signInManager;
        public ProfileController(ShoppingDbContext context, UserManager<Users> usermanager, SignInManager<Users> signInManager)
        {
            _context = context;
            this.usermanager = usermanager;
            this.signInManager = signInManager;
        }
       [Authorize]
        [HttpGet]
        [Route("GetUserDetails")]
        public async Task<Object> getCurrentUser()
        {
            
            var curUser = await usermanager.GetUserAsync(HttpContext.User);
            return curUser;
           
        }
        [HttpPost]
        [Route("updateinfo")]
        [Authorize]
        public async Task<object> updateUserData(UserViewModel model)
        {

           
            var user = _context.Users.Where(u => u.Email == model.email).FirstOrDefault();
            //context.Entry(appuser).State = EntityState.Modified;
            user.Email = model.email;
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.UserName = model.Firstname + "_" + model.Lastname;
            user.Gender = model.Gender;
            
           await _context.SaveChangesAsync();
             try
            {
                var result = await _context.SaveChangesAsync();

                return Ok(result);
            }
            catch (Exception e)
            {
                return e;
            }

        }
        [HttpPost]
        [Authorize]
        [Route("changeuserphoto")]
        public async Task<object> changePhoto(UserViewModel model)
        {


            var user = _context.Users.Where(u => u.Email == model.email).FirstOrDefault();
            
            user.Url = model.Url;

            await _context.SaveChangesAsync();
            try
            {
                var result = await _context.SaveChangesAsync();

                return Ok(user);
            }
            catch (Exception e)
            {
                return e;
            }

        }
    }
}