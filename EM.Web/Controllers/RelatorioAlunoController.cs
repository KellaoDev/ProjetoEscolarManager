using EM.Domain;
using EM.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class RelatorioAlunoController(IRelatorioAluno relatorioAluno) : Controller
    {
        private readonly IRelatorioAluno _relatorioAlunos = relatorioAluno;

        [HttpGet("RelatorioAluno")]
        public IActionResult RelatorioDeAluno()
        {
            return View(new ParametrosRelatorioAluno());
        }

        public IActionResult EmitirRelatorio(ParametrosRelatorioAluno parametros)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", parametros);
            }

            byte[] pdfBytes = _relatorioAlunos.Emita(parametros);

            return File(pdfBytes, "application/pdf", "RelatorioAlunos.pdf");
        }
    }
}
