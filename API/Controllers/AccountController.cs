using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
    public class AccountController:BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService ){
            _tokenService = tokenService;
            _context=context;
        }
        [HttpPost("register")]
        /*khai bao Rgister thuoc kieu AppUser giup tranh tac nghen khi chay khong dong bo*/
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {   if(await UserExists(registerDto.UserName))return BadRequest("Username is taken");
            using var hmac = new HMACSHA512();
            var user = new AppUser{
                UserName=registerDto.UserName.ToLower(), 
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),//ma hoa bang ham bam
                PasswordSalt=hmac.Key//them chuoi Salting vao chuoi de tang kha nang bao mat va tranh bi va cham voi chuoi khac
            };
            _context.Users.Add(user);//chen du lieu user vao database khi SaveChangeAsync() duoc goi
            await _context.SaveChangesAsync();//luu thay doi du lieu tren _context(hay doi tuong nguoi dung )
            return new UserDto
            {
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName==username.ToLower()); // AnyAsysnc : tra lai ket qua true/false khong dong bo 
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user=await _context.Users
            .Include(p=>p.Photos)
            .SingleOrDefaultAsync(x=>x.UserName==loginDto.Username);
            //ham tra ve khong dong bo don gia tri thuoc kieu DataContext la doi tuong Users co ngoai le default
            if(user==null) return Unauthorized("Invalid username");
            using var hmac=new HMACSHA512(user.PasswordSalt);
            var computeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i=0;i<computeHash.Length;i++){
                if(computeHash[i]!=user.PasswordHash[i]){
                    return Unauthorized("Invalid password!");
                }
            }
            return new UserDto
            {
                Username=user.UserName,
                Token=_tokenService.CreateToken(user),
                PhotoUrl=user.Photos.FirstOrDefault(x=>x.IsMain)?.Url
            };
        }
         
        }
    }
