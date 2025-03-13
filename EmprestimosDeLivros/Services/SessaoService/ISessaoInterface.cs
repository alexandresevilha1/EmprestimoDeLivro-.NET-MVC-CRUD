using EmprestimosDeLivros.Models;

namespace EmprestimosDeLivros.Services.SessaoService
{
    public interface ISessaoInterface
    {
        UsuarioModel BuscarSessao();
        void CriarSessao(UsuarioModel usuarioModel);
        void RemoveSessao();
    }
}
