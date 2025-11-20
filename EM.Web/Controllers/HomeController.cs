using EM.Domain;
using EM.Repository.Interfaces;
using EM.Web.Convertes;
using EM.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class HomeController(IRepositorioAluno repositorioAluno) : Controller
    {
        private readonly IRepositorioAluno _repositorioAluno = repositorioAluno;

        public IActionResult Index()
        {
            IEnumerable<Aluno> alunos = _repositorioAluno.GetAll();
            IEnumerable<AlunoModel> alunosModel = alunos.Select(a => a.Converta());

            return View(alunosModel);
        }

        public IActionResult Buscar(string termoPesquisa, string tipoPesquisa)
        {
            IEnumerable<Aluno> alunos = [];

            if (string.IsNullOrWhiteSpace(tipoPesquisa) || tipoPesquisa == "todos")
            {
                alunos = _repositorioAluno.GetAll();
            }
            else if (!string.IsNullOrWhiteSpace(termoPesquisa))
            {
                if (tipoPesquisa == "matricula" && int.TryParse(termoPesquisa, out int matricula))
                {
                    alunos = _repositorioAluno.Get(a => a.Matricula == matricula);
                }
                else if (tipoPesquisa == "nome")
                {
                    alunos = _repositorioAluno.GetByContendoNoNome(termoPesquisa);
                }
            }

            IEnumerable<AlunoModel> alunosModel = alunos.Select(a => a.Converta());

            if (!alunosModel.Any())
            {
                ViewBag.Mensagem = "Nenhum aluno encontrado para a pesquisa informada.";
            }

            return View("Index", alunosModel);
        }
    }
}
