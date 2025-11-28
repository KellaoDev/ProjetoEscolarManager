using EM.Repository.Interfaces;
using EM.Web.DTOs;
using EM.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class RelatorioAlunoController(IRelatorioAlunoService relatorioAluno, IRepositorioCidade repositorioCidade) : BaseController
    {
        private readonly IRelatorioAlunoService _relatorioAlunoService = relatorioAluno;
        private readonly IRepositorioCidade _repositorioCidade = repositorioCidade;

        [HttpGet("RelatorioAluno")]
        public IActionResult RelatorioDeAluno()
        {
            ViewBag.Cidades = _repositorioCidade.GetAll();
            return View(new RelatorioAlunoFiltrosDTO());
        }

        [HttpPost]
        public IActionResult EmitirRelatorio(RelatorioAlunoFiltrosDTO parametros)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Cidades = _repositorioCidade.GetAll();
                return View("RelatorioDeAluno", parametros);
            }

            byte[] pdfBytes = _relatorioAlunoService.Emita(parametros);

            if (pdfBytes is null)
            {
                TempData["Mensagem"] = "Nenhum aluno encontrado para os filtros informados.";
                ViewBag.Cidades = _repositorioCidade.GetAll();
                return View("RelatorioDeAluno", parametros);
            }

            return File(pdfBytes, "application/pdf", "RelatorioAlunos.pdf");
        }
    }
}
