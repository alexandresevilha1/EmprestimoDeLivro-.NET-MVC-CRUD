using EmprestimosDeLivros.Models;
using System.Data;

namespace EmprestimosDeLivros.Services.EmprestimoService
{
    public interface IEmprestimoInterface
    {
        Task<ResponseModel<List<EmprestimosModel>>> BuscarEmprestimos();
        Task<ResponseModel<EmprestimosModel>> BuscarEmprestimoPorId(int id);
        Task<ResponseModel<EmprestimosModel>> CadastrarEmprestimo(EmprestimosModel emprestimosModel);
        Task<ResponseModel<EmprestimosModel>> EditarEmprestimo(EmprestimosModel emprestimosModel);
        Task<ResponseModel<EmprestimosModel>> RemoveEmprestimo(EmprestimosModel emprestimosModel);
        Task<DataTable> BuscaDadosEmprestimosExcel();
    }
}
