using EM.Web.DTOs;
using EM.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class RelatorioAlunoController(IRelatorioAlunoService relatorioAluno) : BaseController
    {
        private readonly IRelatorioAlunoService _relatorioAlunoService = relatorioAluno;

        [HttpGet("RelatorioAluno")]
        public IActionResult RelatorioDeAluno()
        {
            return View(new RelatorioAlunoFiltrosDTO());
        }

        public IActionResult EmitirRelatorio(RelatorioAlunoFiltrosDTO parametros)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", parametros);
            }

            byte[] pdfBytes = _relatorioAlunoService.Emita(parametros);

            return File(pdfBytes, "application/pdf", "RelatorioAlunos.pdf");
        }
    }
}
