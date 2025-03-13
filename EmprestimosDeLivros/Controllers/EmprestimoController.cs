using ClosedXML.Excel;
using EmprestimosDeLivros.Data;
using EmprestimosDeLivros.Models;
using EmprestimosDeLivros.Services.SessaoService;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmprestimosDeLivros.Controllers
{
    public class EmprestimoController : Controller
    {

        readonly private ApplicationDbContext _db;
        readonly private ISessaoInterface _sessaoInterface;

        public EmprestimoController(ApplicationDbContext db, ISessaoInterface sessaoInterface)
        {
            _db = db;
            _sessaoInterface = sessaoInterface;
        }

        public IActionResult Index()
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if(usuario == null)
            {
                return RedirectToAction("Login","Login");
            }
            IEnumerable<EmprestimosModel> emprestimos = _db.Emprestimos;
            return View(emprestimos);
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

        public IActionResult Editar(int? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null || id ==0)
            {
                return NotFound();
            }
            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if(emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        public IActionResult Excluir(int? id)
        {
            var usuario = _sessaoInterface.BuscarSessao();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null || id == 0)
            {
                return NotFound();
            }
            EmprestimosModel emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo == null)
            {
                return NotFound();
            }

            return View(emprestimo);
        }

        public IActionResult Exportar()
        {
            var dados = GetDados();

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

        private DataTable GetDados()
        {
            DataTable dataTable = new DataTable();

            dataTable.TableName = "Resumo de Dados";
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Recebedor", typeof(string));
            dataTable.Columns.Add("Fornecedor", typeof(string));
            dataTable.Columns.Add("Livro", typeof(string));
            dataTable.Columns.Add("Data empréstimo", typeof(DateTime));

            var dados = _db.Emprestimos.ToList();
            if(dados.Count() > 0)
            {
                dados.ForEach(emprestimo =>
                {
                    dataTable.Rows.Add(emprestimo.Id, emprestimo.Recebedor, emprestimo.Fornecedor, emprestimo.LivroEmprestado, emprestimo.DataAtualizacao);
                });
            }

            return dataTable;
        }

        [HttpPost]
        public IActionResult Cadastrar(EmprestimosModel emprestimos)
        {
            if(ModelState.IsValid)
            {
                emprestimos.DataAtualizacao = DateTime.Now;

                _db.Emprestimos.Add(emprestimos);
                _db.SaveChanges();
                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";    
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Editar(EmprestimosModel emprestimo)
        {
            if(ModelState.IsValid)
            {
                var EmprestimoDB = _db.Emprestimos.Find(emprestimo.Id);

                EmprestimoDB.Fornecedor = emprestimo.Fornecedor;
                EmprestimoDB.Recebedor = emprestimo.Recebedor;
                EmprestimoDB.LivroEmprestado = emprestimo.LivroEmprestado;

                _db.Emprestimos.Update(emprestimo);
                _db.SaveChanges();
                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";
                return RedirectToAction("Index");
            }
            return View(emprestimo);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel emprestimo)
        {
            if (emprestimo == null)
            {
                return NotFound();
            }
            _db.Emprestimos.Remove(emprestimo);
            _db.SaveChanges();
            TempData["MensagemSucesso"] = "Remoção realizada com sucesso!";
            return RedirectToAction("Index");
        }
    }
}
