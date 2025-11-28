using EM.Domain;
using EM.Repository.Interfaces;
using EM.Web.Convertes;
using EM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EM.Web.Controllers
{
    public class AlunoController(IRepositorioAluno repositorioAluno, IRepositorioCidade repositorioCidade) : BaseController
    {
        private readonly IRepositorioAluno _repositorioAluno = repositorioAluno;
        private readonly IRepositorioCidade _repositorioCidade = repositorioCidade;

        public IActionResult Salvar(int? id)
        {
            if (id is null)
            {
                CarregarCidadesDropDown(null);
                return View("Aluno", new AlunoModel());
            }
            Aluno? aluno = _repositorioAluno.GetByMatricula(id.Value);

            AlunoModel alunoModel = aluno.Converta();
            CarregarCidadesDropDown(alunoModel.Cidade?.Codigo);

            return View("Aluno", alunoModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(AlunoModel alunoModel)
        {
            CarregarCidadesDropDown(alunoModel.Cidade?.Codigo);

            if (alunoModel.Cidade?.Codigo > 0)
            {
                Cidade cidade = _repositorioCidade.GetByCodigo(alunoModel.Cidade.Codigo);

                if (cidade is not null)
                {
                    alunoModel.Cidade.Descricao = cidade.Descricao;
                    alunoModel.Cidade.EnumeradorUF = cidade.EnumeradorUF;

                    ModelState.Remove("Cidade.Descricao");
                    ModelState.Remove("Cidade.EnumeradorUF");
                }
            }

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
            CarregarCidadesDropDown(alunoModel.Cidade?.Codigo);

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

        private void CarregarCidadesDropDown(int? cidadeSelecionadaId = null)
        {
            var listaCidade = _repositorioCidade.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = $"{c.Descricao} - {c.EnumeradorUF}",
                    Value = c.Codigo.ToString(),
                    Selected = cidadeSelecionadaId.HasValue && cidadeSelecionadaId.Value == c.Codigo
                })
                .ToList();

            ViewBag.Cidades = listaCidade;
        }
    }
}