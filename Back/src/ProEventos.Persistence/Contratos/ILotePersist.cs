using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {    
        /// <summary>
        /// Método Get que retornará uma lista de lotes por EventoId.
        /// </summary>
        /// <param name="eventoId"> Código chave da tabela Evento</param>
        /// <returns> Lista (Array) de Lotes </returns>
        Task<Lote[]> GetLotesByEventoIdAsync(int eventoId);
        
        /// <summary>
        /// Métodos Get que retornará apenas 1 lote
        /// </summary>
        /// <param name="eventoId"> Código chave da tabela Evento</param>
        /// <param name="id"> Código chave da tabela Lote</param>
        /// <returns>Apenas 1 lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int id);
    }
}