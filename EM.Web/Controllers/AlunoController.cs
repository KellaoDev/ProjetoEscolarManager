using EM.Domain;
using EM.Repository.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

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
                ViewBag.IsEdicao = false;
                return View("Aluno", new Aluno());
            }
            ViewBag.IsEdicao = true;
            Aluno? aluno = _repositorioAluno.Get(c => c.Matricula == id).FirstOrDefault();

            return View("Aluno", aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(Aluno aluno)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IsEdicao = aluno.Matricula > 0;
                ViewBag.Cidades = _repositorioCidade.GetAll().ToList();
                return View("Aluno", aluno);
            }

            if (_repositorioAluno.CpfExiste(aluno.Cpf, aluno.Matricula))
            {
                ModelState.AddModelError("Cpf", "Já existe um aluno cadastrado com esse CPF.");
                ViewBag.IsEdicao = false;
                ViewBag.Cidades = _repositorioCidade.GetAll().ToList();
                return View("Aluno", aluno);
            }
            
            if (aluno.Matricula > 0)
            {
                _repositorioAluno.Update(aluno);
            }
            else
            {
                _repositorioAluno.Add(aluno);
            }

            TempData["MensagemSucesso"] = "Aluno salvo com sucesso! ✅";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Remove(int id)
        {
            Aluno aluno = _repositorioAluno.Get(c => c.Matricula == id).First();
            _repositorioAluno.Remove(aluno);

            TempData["MensagemSucesso"] = "Aluno excluido com sucesso! ✅";
            return RedirectToAction("Index", "Home");
        }
    }
}