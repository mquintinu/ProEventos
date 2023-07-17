using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;        
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IMapper mapper,
                              IUserPersist userPersist)
        {
            _signInManager = signInManager;
            _mapper = mapper;
            _userManager = userManager;
            _userPersist = userPersist;
        }

        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDTO, string password)
        {
            try
            {
                 var user = await _userManager.Users
                                            .SingleOrDefaultAsync(user => user.UserName == userUpdateDTO.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (System.Exception ex)
            {            
                throw new Exception($"Erro ao tentar verificar password. Erro: {ex.Message}");
            };
        }

        public async Task<UserUpdateDTO> CreateAccountAsync(UserDTO userDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userDTO);
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserUpdateDTO>(user);
                    return userToReturn;
                }

                return null;
            }
            catch (System.Exception ex)
            {            
                throw new Exception($"Erro ao tentar criar usuário. Erro: {ex.Message}");
            };
        }

        public async Task<UserUpdateDTO> GetUserByUserNameAsync(string userName)
        {
            try
            {
                 var user = await _userPersist.GetUserByUserNameAsync(userName);
                 if (user == null) return null;

                 var userUpdateDTO = _mapper.Map<UserUpdateDTO>(user);
                 return userUpdateDTO;
            }
            catch (System.Exception ex)
            {            
                throw new Exception($"Erro ao tentar consultar usuário por username. Erro: {ex.Message}");
            };
        }

        public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await _userPersist.GetUserByUserNameAsync(userUpdateDTO.UserName);
                if (user == null) return null;

                userUpdateDTO.Id = user.Id;

                _mapper.Map(userUpdateDTO, user);

                if (userUpdateDTO.Password != null) {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDTO.Password);
                }
                 _userPersist.Update<User>(user);

                 if (await _userPersist.SaveChangesAsync())
                 {
                    var userRetorno = await _userPersist.GetUserByUserNameAsync(user.UserName);

                    return _mapper.Map<UserUpdateDTO>(userRetorno);
                 }
                 
                 return null;                
            }
            catch (System.Exception ex)
            {            
                throw new Exception($"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
            };
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager
                                    .Users
                                    .AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (System.Exception ex)
            {            
                throw new Exception($"Erro ao tentar verificar se usuário existe. Erro: {ex.Message}");
            };
        }
    }
}