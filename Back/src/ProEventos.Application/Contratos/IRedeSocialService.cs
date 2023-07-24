using System.Threading.Tasks;
using ProEventos.Application.DTOs;

namespace ProEventos.Application.Contratos
{
    public interface IRedeSocialService
    {
        Task<RedeSocialDTO[]> SaveByEvento(int eventoId, RedeSocialDTO[] models);
        Task<bool> DeleteByEvento(int eventoId, int id);
        Task<RedeSocialDTO[]> SaveByPalestrante(int palestranteId, RedeSocialDTO[] models);
        Task<bool> DeleteByPalestrante(int palestranteId, int redeSocialId);
        Task<RedeSocialDTO[]> GetAllByEventoIdAsync(int eventoId);
        Task<RedeSocialDTO[]> GetAllByPalestranteIdAsync(int palestranteId);
        Task<RedeSocialDTO> GetRedeSocialEventoByIdsAsync(int eventoId, int redeSocialId);
        Task<RedeSocialDTO> GetRedeSocialPalestranteByIdsAsync(int palestranteId, int redeSocialId);
        
    }
}