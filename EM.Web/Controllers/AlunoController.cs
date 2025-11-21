using EM.Domain;
using EM.Web.Models;
using EM.Web.Convertes;
using EM.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class AlunoController(IRepositorioAluno repositorioAluno, IRepositorioCidade repositorioCidade) : BaseController
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
            Aluno? aluno = _repositorioAluno.GetByMatricula(id.Value);

            AlunoModel alunoModel = aluno.Converta();
            return View("Aluno", alunoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(AlunoModel alunoModel)
        {
            ViewBag.Cidades = _repositorioCidade.GetAll().ToList();

            if (!ModelState.IsValid)
            {
                return View("Aluno", alunoModel);
            }

            if (_repositorioAluno.CpfExiste(alunoModel.Cpf, alunoModel.Matricula))
            {
                ModelState.AddModelError("Cpf", "Já existe um aluno cadastrado com esse CPF.");
                return View("Aluno", alunoModel);
            }

            Aluno aluno = alunoModel.Converta();

            if (aluno.Matricula > 0)
            {
                _repositorioAluno.Update(aluno);
            }
            else
            {
                _repositorioAluno.Add(aluno);
            }

            TempData["MensagemSucesso"] = "Aluno salvo com sucesso! ✅";
            return Redirecionar("Index", "Home");
        }

        public IActionResult Remove(int id)
        {
            Aluno aluno = _repositorioAluno.GetByMatricula(id);
            _repositorioAluno.Remove(aluno);

            TempData["MensagemSucesso"] = "Aluno excluido com sucesso! ✅";
            return Redirecionar("Index", "Home");
        }
    }
}