using System.ComponentModel.DataAnnotations;

namespace EmprestimosDeLivros.DTO
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "Digite o Email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite a Senha!")]
        public string Senha { get; set; }
    }
}
