using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PalestrantesController : ControllerBase
    {
        
        public IPalestranteService _palestranteService { get; }
        public IWebHostEnvironment _hostEnvironment { get; }
        public IAccountService _accountService { get; }

        public PalestrantesController(IPalestranteService palestranteService, 
                                      IWebHostEnvironment hostEnvironment,
                                      IAccountService accountService) { 
            this._palestranteService = palestranteService;
            this._hostEnvironment = hostEnvironment;
            this._accountService = accountService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] PageParams pageParams)
        {        
            try
            {
                var palestrante = await _palestranteService.GetAllPalestrantesAsync(pageParams, true);
                if (palestrante == null) return NoContent();

                Response.AddPagination(palestrante.CurrentPage, palestrante.PageSize,
                                       palestrante.TotalCount, palestrante.TotalPages);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }

        [HttpGet]        
        public async Task<IActionResult> GetPalestrantes()
        {        
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), true);
                if (palestrante == null) return NoContent();                

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao tentar recuperar palestrantes. Erro: {ex.Message}");
            }
        }   
        
        [HttpPost]
        public async Task<IActionResult> Post(PalestranteAddDTO model)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null)
                    palestrante = await _palestranteService.AddPalestrante(User.GetUserId(), model);

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar adicionar Palestrantes. Erro: {ex.Message}");
            }
        }      

        [HttpPut]
        public async Task<IActionResult> Put(PalestranteUpdateDTO model)
        {
            try
            {
                var palestrante = await _palestranteService.UpdatePalestrante(User.GetUserId(), model);
                if (palestrante == null) return BadRequest("Erro ao tentar atualizar o Palestrante.");

                return Ok(palestrante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, 
                    $"Erro ao tentar Palestranteeventos. Erro: {ex.Message}");
            }
        }        
    }
}
