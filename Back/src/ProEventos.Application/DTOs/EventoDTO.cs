using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.DTOs
{
    public class EventoDTO
    {
        public int Id { get; set; }

        public string Local { get; set; }

        public string DataEvento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        // [MinLength(4, ErrorMessage = "{0} = o campo deve ter no mínimo 4 caracteres.")]
        // [MaxLength(50, ErrorMessage = "{0} = o campo deve ter no máximo 50 caracteres.")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} = O tema deve ter de 4 a 50 caracteres.")]
        public string Tema { get; set; }
    
        [Display(Name = "quantidade de pessoas")]
        [Range(1, 120000, ErrorMessage = "{0} não pode ser menor que 1 e maior que 120.000")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Não é uma imagem válida (.gif, .png, .jpg ou bmp).")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {} é obrigatório.")]
        [Phone(ErrorMessage = "O campo {} está com número inválido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
         Display(Name = "e-mail"),
         EmailAddress(ErrorMessage ="É necessário ser um {0} válido.")]
        public string Email { get; set; }

        public IEnumerable<LoteDTO> Lotes { get; set; }
        public IEnumerable<RedeSocialDTO> RedesSociais { get; set; }
        public IEnumerable<PalestranteDTO> PalestrantesEventos { get; set; }
    }
}