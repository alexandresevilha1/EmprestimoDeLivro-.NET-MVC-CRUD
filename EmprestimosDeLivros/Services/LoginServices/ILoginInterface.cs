using EmprestimosDeLivros.DTO;
using EmprestimosDeLivros.Models;

namespace EmprestimosDeLivros.Services.LoginServices
{
    public interface ILoginInterface
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto);
    }
}
