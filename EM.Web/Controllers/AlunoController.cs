using EM.Web.Models;
using EM.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EM.Web.Convertes;
using EM.Domain;

namespace EM.Web.Controllers
{
    public class AlunoController(IRepositorioAluno repositorioAluno, IRepositorioCidade repositorioCidade) : Controller
    {
        private readonly IRepositorioAluno _repositorioAluno = repositorioAluno;
        private readonly IRepositorioCidade _repositorioCidade = repositorioCidade;

        public IActionResult Salvar(int? id)
        {
            ViewBag.Cidades = _repositorioCidade.GetAll().ToList();
            if (id is null)
            {
                return View("Aluno", new AlunoModel());
            }
            Aluno? entidade = _repositorioAluno.GetByMatricula(id.Value);

            AlunoModel model = entidade.Converta();
            return View("Aluno", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(AlunoModel aluno)
        {
            ViewBag.Cidades = _repositorioCidade.GetAll().ToList();

            if (!ModelState.IsValid)
            {
                return View("Aluno", aluno);
            }

            if (_repositorioAluno.CpfExiste(aluno.Cpf, aluno.Matricula))
            {
                ModelState.AddModelError("Cpf", "Já existe um aluno cadastrado com esse CPF.");
                return View("Aluno", aluno);
            }

            Aluno entidade = aluno.Converta();

            if (aluno.Matricula > 0)
            {
                _repositorioAluno.Update(entidade);
            }
            else
            {
                _repositorioAluno.Add(entidade);
            }

            TempData["MensagemSucesso"] = "Aluno salvo com sucesso! ✅";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Remove(int id)
        {
            Aluno entidade = _repositorioAluno.GetByMatricula(id);
            _repositorioAluno.Remove(entidade);

            TempData["MensagemSucesso"] = "Aluno excluido com sucesso! ✅";
            return RedirectToAction("Index", "Home");
        }
    }
}