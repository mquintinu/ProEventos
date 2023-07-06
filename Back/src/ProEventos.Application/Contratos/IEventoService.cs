using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDTO> AddEvento(EventoDTO model);
        Task<EventoDTO> UpdateEventos(int eventoId, EventoDTO model);
        Task<bool> DeleteEvento(int eventoId);

        Task<EventoDTO[]> GetAllEventosAsync(bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false);
        Task<EventoDTO> GetEventoByIdAsync(int EventoId, bool includePalestrantes = false);        
    }
}