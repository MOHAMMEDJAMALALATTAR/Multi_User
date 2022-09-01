using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Multi_User.Models;
using LoginModel = Multi_User.Models.LoginModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Multi_User.Data;
using System.Linq;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections;

namespace Multi_User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Multi_UsersController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _env;

        public Multi_UsersController(UserManager<ApiUser> userManager, SignInManager<ApiUser> signInManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;

        }

        [HttpGet]
        public IEnumerable<ApiUser> GetAll()
        {
            var Listall = _context.ApiUsers.ToList();
            return Listall;
        }
        [HttpGet ]
        public IEnumerable <ApiUser> Get(int Id)
        {
            var singlePost = _context.ApiUsers.Find(Id);
            return (IEnumerable<ApiUser>)singlePost;
        }
        [HttpPost]
        public async Task<int> CreateAsync(ApiUser user)
        {
            var NameIsExist = _context.ApiUsers.Any(x => x.FullName == user.FullName);
            if (NameIsExist)
            {
                throw new Exception();

            }
            string filePath = "";
            // uploud image 
            if (user.Logo != null)
            {
                filePath = "Image/Logos/" + Guid.NewGuid().ToString() + "_" + user.Logo.FileName;
                string serverFolder = Path.Combine(_env.WebRootPath, filePath);
                await user.Logo.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }

            var AddPost = new ApiUser();
            AddPost.FullName = user.FullName;
            AddPost.Title = user.Title;
            AddPost.Subtitle=user.Subtitle;
            AddPost.CreationDate= user.CreationDate;

            _context.SaveChanges();
            return AddPost.Id;

        }
        [HttpPut]
        public async Task<int> UpdateAsync(ApiUser user)
        {
            var IsExist = _context.ApiUsers.Find(user.Id);
            string filePath = user.LogoPath;
            // uploud image 
            if (user.Logo != null)
            {
                filePath = "Image/Logos/" + Guid.NewGuid().ToString() + "_" + user.Logo.FileName;
                string serverFolder = Path.Combine(_env.WebRootPath, filePath);
                await user.Logo.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }


            var UpdatePost = new ApiUser();
            UpdatePost.FullName = user.FullName;
            UpdatePost.Title = user.Title;
            UpdatePost.Subtitle = user.Subtitle;
            UpdatePost.CreationDate = user.CreationDate;

            _context.SaveChanges();
            return UpdatePost.Id;

        }
        [HttpDelete]
        public void Delete(int Id)
        {
            var oldValue = _context.ApiUsers.Find(Id);

            _context.Remove(oldValue);
            _context.SaveChanges();

        }

        [HttpGet]
        public IEnumerable Login()
        {
            yield return Ok();
        }
        [HttpPost]
        public async Task<IEnumerable> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var Result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (Result.Succeeded)
                    {
                        return (IEnumerable)RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("SignIn", "Password or User Name is Wrong");
                    }
                }
                else
                {
                    ModelState.AddModelError("SignIn", "you are not valid");
                }
            }

            return (IEnumerable)Ok(model);
        }




        [HttpGet]
        public async Task<IEnumerable> Logout()
        {
            await _signInManager.SignOutAsync();
            return (IEnumerable)RedirectToAction("Index", "Home");
        }
    }
}
