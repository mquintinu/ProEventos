using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.DTOs;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {        
        private readonly IPalestrantePersist _palestrantePersist;
        public IMapper _mapper { get; }

        public PalestranteService(IGeralPersist geralPersist, IPalestrantePersist palestrantePersist,
                             IMapper mapper)
        {
            this._palestrantePersist = palestrantePersist;
            this._mapper = mapper;
        }

        public async Task<PalestranteDTO> AddPalestrante(int userId, PalestranteAddDTO model)
        {            
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantePersist.Add<Palestrante>(palestrante);

                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDTO>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDTO> UpdatePalestrante(int userId, PalestranteUpdateDTO model)
        {            
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);
                if (palestrante == null) return null;

                model.Id = palestrante.Id;
                model.UserId = userId;

                _mapper.Map(model, palestrante);

                _palestrantePersist.Update<Palestrante>(palestrante);

                if (await _palestrantePersist.SaveChangesAsync())
                {
                    var palestranteRetorno = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);

                    return _mapper.Map<PalestranteDTO>(palestranteRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        

        public async  Task<PageList<PalestranteDTO>> GetAllPalestrantesAsync(PageParams pageParams, bool includeEventos = false)
        {
            try
            {
                 var palestrante = await _palestrantePersist.GetAllPalestrantesAsync(pageParams, includeEventos);
                 if (palestrante == null) return null;
                    
                 var resultado = _mapper.Map<PageList<PalestranteDTO>>(palestrante);

                 resultado.CurrentPage = palestrante.CurrentPage;
                 resultado.TotalPages = palestrante.TotalPages;
                 resultado.PageSize = palestrante.PageSize;
                 resultado.TotalCount = palestrante.TotalCount;

                 return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDTO> GetPalestranteByUserIdAsync(int userId, bool includeEventos = false)
        {
            try
            {
                 var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, includeEventos);
                 if (palestrante == null) return null;

                 var resultado = _mapper.Map<PalestranteDTO>(palestrante);

                 return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}