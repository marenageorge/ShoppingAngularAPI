using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
//using System.Net.Http;

namespace AngularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class UsersController : ControllerBase
    {
        private readonly ShoppingDbContext _context;
        private readonly UserManager<Users> usermanager;
        private readonly ApplicationSettings options;

        private readonly SignInManager<Users> signInManager;

        public RoleManager<IdentityRole> RoleManager { get; }

        public UsersController(ShoppingDbContext context, UserManager<Users> usermanager, SignInManager<Users> signInManager,IOptions<ApplicationSettings>options,RoleManager<IdentityRole>roleManager)
        {
            _context = context;
            this.usermanager = usermanager;
            this.signInManager = signInManager;
            RoleManager = roleManager;
            this.options = options.Value ;
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("Upload")]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<object> PostUser(UserViewModel model)
        {
           
                var user = new Users()
            {
                UserName = model.Firstname+"_"+ model.Lastname,
                Email= model.email,
                Gender= model.Gender,
                Url= model.Url,
                Firstname=model.Firstname,
                Lastname=model.Lastname

            };
            try
            {
                var result = await usermanager.CreateAsync(user, model.password);
                var role = new IdentityRole();
                role.Name = "User";
                if (model.email=="admin@gmail.com" && model.password == "Admin@123")
                {
                    role.Name = "Admin";
                    if (await RoleManager.RoleExistsAsync(role.Name) == false)
                        await RoleManager.CreateAsync(role);

                    await usermanager.AddToRoleAsync(user, role.Name);
                }
              

               else if( await RoleManager.RoleExistsAsync(role.Name)==false)
                await RoleManager.CreateAsync(role);

                await usermanager.AddToRoleAsync(user, role.Name);

                return Ok(result);
            }
            catch (Exception e)
            {
                return e;
            }
        }
        [HttpPost]
        [Route("resetpass")]
        public async Task<IActionResult> resetpass(UserViewModel model)
        {
            var user = _context.users.FirstOrDefault(i => i.Email == model.email);
            if (user != null) {
                var code = await usermanager.GeneratePasswordResetTokenAsync(user);
                var result = await usermanager.ResetPasswordAsync(user, code, model.password);
                return Ok();
            }
            return NotFound();
        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> login(UserViewModel model)
        {
           
            var user = _context.users.FirstOrDefault(i => i.Email == model.email);
            var userRole = await usermanager.GetRolesAsync(user);
            if (user != null && await usermanager.CheckPasswordAsync(user, model.password))
            {

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                    new Claim(JwtRegisteredClaimNames.Typ,userRole[0]),

                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecurityKey"));
                var token = new JwtSecurityToken(
                    issuer: "http://oec.com",
                    audience: "http://oec.com",
                    expires: DateTime.UtcNow.AddHours(5),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo,role=userRole[0] });
            }
            else
                return NotFound();
        }
      
     
    }
}