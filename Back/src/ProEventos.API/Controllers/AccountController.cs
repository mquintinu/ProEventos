using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public ITokenService _tokenService;
        public AccountController(IAccountService accountService,        
                                ITokenService tokenService)
        {
            this._tokenService = tokenService;
            this._accountService = accountService;            
        }


        [HttpGet("GetUser")]        
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (System.Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar recuperar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            try
            {
                if (await _accountService.UserExists(userDTO.UserName))
                    return BadRequest("Usuário já existe!");
                
                var user = await _accountService.CreateAccountAsync(userDTO);

                if (user != null)
                    return Ok(user);

                return BadRequest("Usuário não criado, tente novamente mais tarde.");
            }
            catch (System.Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar cadastrar Usuário. Erro: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDTO userLogin)
        {
            try
            {
               var user = await _accountService.GetUserByUserNameAsync(userLogin.Username);
               if (user == null) return Unauthorized("Usuário ou Senha incorreto.");

               var result = await _accountService.CheckUserPasswordAsync(user, userLogin.Password);
               if (!result.Succeeded) return Unauthorized();

               return Ok(new 
               {
                    userName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
               });
            }
            catch (System.Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar realizar o Login. Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            try
            {
               var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
               if (user == null) return Unauthorized("Usuário inválido.");

               var userReturn = await _accountService.UpdateAccount(userUpdateDTO);
               if (userReturn == null) return NoContent();

               return Ok(userReturn);
            }
            catch (System.Exception ex)
            {                
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar atualizar Usuário. Erro: {ex.Message}");
            }
        }        
    }
}