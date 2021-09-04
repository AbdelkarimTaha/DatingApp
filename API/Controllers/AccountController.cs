﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(string userName, string password) //because we using [ApiController] we don't need [FormBody]
        {
            using var hmac = new HMACSHA512(); //using statment because when finish with this class, it's disposed correctly  

            var user = new AppUser
            {
                UserName = userName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
