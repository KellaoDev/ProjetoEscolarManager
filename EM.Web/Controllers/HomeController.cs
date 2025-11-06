using EM.Repository.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using EM.Domain;

namespace EM.Web.Controllers
{
    public class HomeController(IRepositorioAluno repositorioAluno) : Controller
    {
        private readonly IRepositorioAluno _repositorioAluno = repositorioAluno;

        public IActionResult Index()
        {
            IEnumerable<Aluno> listaAlunos = _repositorioAluno.GetAll();
            return View(listaAlunos);
        }

        [HttpGet]
        public IActionResult Buscar(string termoPesquisa, string tipoPesquisa)
        {
            IEnumerable<Aluno> listaAlunos = [];

            if (string.IsNullOrWhiteSpace(tipoPesquisa) || tipoPesquisa == "todos")
            {
                listaAlunos = _repositorioAluno.GetAll();
            }
            else if (!string.IsNullOrWhiteSpace(termoPesquisa))
            {
                if (tipoPesquisa == "matricula" && int.TryParse(termoPesquisa, out int mat))
                {
                    listaAlunos = _repositorioAluno.Get(a => a.Matricula == mat);
                }
                else if (tipoPesquisa == "nome")
                {
                    listaAlunos = _repositorioAluno.Get(a => a.Nome.Contains(termoPesquisa, System.StringComparison.OrdinalIgnoreCase));
                }
            }

            if (!listaAlunos.Any())
            {
                ViewBag.Mensagem = "Nenhum aluno encontrado para a pesquisa informada.";
            }

            return View("Index", listaAlunos);
        }

    }

}
