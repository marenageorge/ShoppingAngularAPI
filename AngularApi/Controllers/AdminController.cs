using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AngularApi.Models;
namespace AngularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<Users> _usermanager;
        private readonly SignInManager<Users> _SignInManager;
        private readonly ShoppingDbContext _context;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<Users> usermanager, SignInManager<Users> SignInManager,
                                       ShoppingDbContext context)
        {
            this._roleManager = roleManager;
            this._usermanager = usermanager;
            _SignInManager = SignInManager;
            _context = context;
        }
        [HttpPost]
        [Route("createRole")]
        public async Task<IActionResult> CreateRole(RoleViewModel vm)
        {
          
                IdentityRole Applicationrole = new IdentityRole() { Name =vm.RoleName};
                var result = await _roleManager.CreateAsync(Applicationrole);
                if (result.Succeeded)
            return Ok();
            return BadRequest();

        }
    }
}