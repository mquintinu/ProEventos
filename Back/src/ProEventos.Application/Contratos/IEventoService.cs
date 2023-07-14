using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDTO> AddEvento(int userId, EventoDTO model);
        Task<EventoDTO> UpdateEventos(int userId, int eventoId, EventoDTO model);
        Task<bool> DeleteEvento(int userId, int eventoId);

        Task<EventoDTO[]> GetAllEventosAsync(int userId, bool includePalestrantes = false);
        Task<EventoDTO[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false);
        Task<EventoDTO> GetEventoByIdAsync(int userId, int EventoId, bool includePalestrantes = false);        
    }
}