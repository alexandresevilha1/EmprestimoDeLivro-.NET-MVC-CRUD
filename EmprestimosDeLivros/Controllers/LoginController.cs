using EmprestimosDeLivros.DTO;
using EmprestimosDeLivros.Services.LoginServices;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosDeLivros.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginInterface _loginInterface;

        public LoginController(ILoginInterface loginInterface)
        {
            _loginInterface = loginInterface;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioRegisterDto usuarioRegisterDto)
        {
            if(ModelState.IsValid)
            {
                var usuario = await _loginInterface.RegistrarUsuario(usuarioRegisterDto);
                if(usuario.Status)
                {
                    TempData["MensagemSucesso"] = usuario.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = usuario.Mensagem;
                    return View(usuarioRegisterDto);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(usuarioRegisterDto);
            }
        }
    }
}
