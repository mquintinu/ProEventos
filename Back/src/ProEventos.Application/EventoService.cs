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
    public class EventoService : IEventoService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IEventoPersist _eventoPersist;
        public IMapper _mapper { get; }

        public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist,
                             IMapper mapper)
        {            
            this._geralPersist = geralPersist; 
            this._eventoPersist = eventoPersist;       
            this._mapper = mapper;
        }

        public async Task<EventoDTO> AddEvento(int userId, EventoDTO model)
        {            
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersist.Add<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);

                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> UpdateEventos(int userId, int eventoId, EventoDTO model)
        {            
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
                if (evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);

                if (await _geralPersist.SaveChangesAsync())
                {
                    var eventoRetorno = await _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);

                    return _mapper.Map<EventoDTO>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {   
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
                if (evento == null) throw new Exception("Evento para delete não foi encontrado.");            

                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesAsync();
            
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async  Task<PageList<EventoDTO>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                 var eventos = await _eventoPersist.GetAllEventosAsync(userId, pageParams, includePalestrantes);
                 if (eventos == null) return null;
                    
                 var resultado = _mapper.Map<PageList<EventoDTO>>(eventos);

                 resultado.CurrentPage = eventos.CurrentPage;
                 resultado.TotalPages = eventos.TotalPages;
                 resultado.PageSize = eventos.PageSize;
                 resultado.TotalCount = eventos.TotalCount;

                 return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDTO> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                 var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId);
                 if (evento == null) return null;

                 var resultado = _mapper.Map<EventoDTO>(evento);

                 return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}