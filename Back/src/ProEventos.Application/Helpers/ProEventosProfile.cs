using AutoMapper;
using ProEventos.Application.DTOs;
using ProEventos.Domain;

namespace ProEventos.Application.Helpers
{
    public class ProEventosProfile : Profile    
    {
        public ProEventosProfile(){
            CreateMap<Evento, EventoDTO>().ReverseMap();
            CreateMap<Lote, LoteDTO>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDTO>().ReverseMap();
            CreateMap<Palestrante, PalestranteDTO>().ReverseMap();
        }
    }
}