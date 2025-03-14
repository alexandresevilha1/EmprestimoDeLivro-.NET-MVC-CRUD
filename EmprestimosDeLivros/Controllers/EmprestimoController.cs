using ClosedXML.Excel;
using EmprestimosDeLivros.Data;
using EmprestimosDeLivros.Models;
using EmprestimosDeLivros.Services.EmprestimoService;
using EmprestimosDeLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace EmprestimosDeLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        private readonly ISessaoInterface _sessaoInterface;
        private readonly IEmprestimoInterface _emprestimosInterface;

        public EmprestimoController(IEmprestimoInterface emprestimoInterface, ISessaoInterface sessaoInterface)
        {
            _sessaoInterface = sessaoInterface;
            _emprestimosInterface = emprestimoInterface;
        }

        public async Task<IActionResult> Index()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if(usuario == null)
            {
                return RedirectToAction("Login","Login");
            }
            var emprestimos = await _emprestimosInterface.BuscarEmprestimos();
            return View(emprestimos.Dados);
        }

        public IActionResult Cadastrar()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var emprestimo = await _emprestimosInterface.BuscarEmprestimoPorId(id);

            return View(emprestimo.Dados);
        }

        public async Task<IActionResult> Excluir(int id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var emprestimo = await _emprestimosInterface.BuscarEmprestimoPorId(id);

            return View(emprestimo);
        }

        public async Task<IActionResult> Exportar()
        {
            var dados = await _emprestimosInterface.BuscaDadosEmprestimosExcel();

            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.AddWorksheet(dados,"Dados Empréstimos");
                using (MemoryStream ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet", "Emprestimo.xls");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(EmprestimosModel emprestimo)
        {
            if(ModelState.IsValid)
            {
                var emprestimoResult = await _emprestimosInterface.CadastrarEmprestimo(emprestimo);
                if(emprestimoResult.Status)
                {
                    TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = emprestimoResult.Mensagem;
                    return View(emprestimo);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(EmprestimosModel emprestimo)
        {
            if(ModelState.IsValid)
            {
                var emprestimoResult = await _emprestimosInterface.EditarEmprestimo(emprestimo);
                if(emprestimoResult.Status)
                {
                    TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
                }
                else
                {
                    TempData["MensagemErro"] = emprestimoResult.Mensagem;
                    return View(emprestimo);
                }

                    return RedirectToAction("Index");
            }
            return View(emprestimo);
        }

        [HttpPost]
        public async Task<IActionResult> Excluir(EmprestimosModel emprestimo)
        {
            if (emprestimo == null)
            {
                TempData["MensagemErro"] = "Emprestimo não localizado";
                return View(emprestimo);
            }
            var emprestimoResult = await _emprestimosInterface.RemoveEmprestimo(emprestimo);
            if(emprestimoResult.Status)
            {
                TempData["MensagemSucesso"] = emprestimoResult.Mensagem;
            }
            else
            {
                TempData["MensagemErro"] = emprestimoResult.Mensagem;
            }

            return RedirectToAction("Index");
        }
    }
}
