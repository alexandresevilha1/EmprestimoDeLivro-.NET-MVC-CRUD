using EmprestimosDeLivros.Data;
using EmprestimosDeLivros.DTO;
using EmprestimosDeLivros.Models;
using EmprestimosDeLivros.Services.SenhaService;
using EmprestimosDeLivros.Services.SessaoService;

namespace EmprestimosDeLivros.Services.LoginServices
{
    public class LoginService : ILoginInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly ISenhaInterface _senhaInterface;
        private readonly ISessaoInterface _sessaoInterface;

        public LoginService(ApplicationDbContext context, ISenhaInterface senhaInterface, ISessaoInterface sessaoInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
            _sessaoInterface = sessaoInterface;
        }

        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLoginDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioLoginDto.Email);
                if(usuario == null)
                {
                    response.Mensagem = "Credenciais inválidas";
                    response.Status = false;
                    return response;
                }
                if(!_senhaInterface.VerificaSenha(usuarioLoginDto.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais inválidas";
                    response.Status = false;
                    return response;
                }
                _sessaoInterface.CriarSessao(usuario);

                response.Mensagem = "usuário logado com sucesso!";

                return response;
            }
            catch(Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

            try
            {
                if(VerificarSeEmailExiste(usuarioRegisterDto))
                {
                    response.Mensagem = "Email Já Cadastrado";
                    response.Status = false;
                    return response;
                }
                _senhaInterface.CriarSenhaHash(usuarioRegisterDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);

                var usuario = new UsuarioModel()
                { 
                    Nome = usuarioRegisterDto.Nome,
                    Sobrenome = usuarioRegisterDto.Sobrenome,
                    Email = usuarioRegisterDto.Email,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                response.Mensagem = "Usuario Cadastrado Com Sucesso!";
                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        private bool VerificarSeEmailExiste(UsuarioRegisterDto usuarioRegisterDto)
        {
            var usuario = _context.Usuarios.FirstOrDefault(x => x.Email == usuarioRegisterDto.Email);
            if(usuario == null)
            {
                return false;
            }

            return true;
        }
    }
}
