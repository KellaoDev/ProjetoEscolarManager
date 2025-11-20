using EM.Domain;
using EM.Repository.Interfaces;
using EM.Web.Convertes;
using EM.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class CidadeController(IRepositorioCidade repositorioCidade) : Controller
    {
        private readonly IRepositorioCidade _repositorioCidade = repositorioCidade;

        public IActionResult ListaCidade()
        {
            IEnumerable<Cidade> cidades = _repositorioCidade.GetAll();
            IEnumerable<CidadeModel> cidadesModel = cidades.Select(c => c.Converta());

            return View(cidadesModel);
        }

        public IActionResult Buscar(string termoPesquisa, string tipoPesquisa)
        {
            IEnumerable<Cidade> cidades = [];

            if(string.IsNullOrWhiteSpace(tipoPesquisa) || tipoPesquisa == "todos")
            {
                cidades = _repositorioCidade.GetAll();
            }
            else if (!string.IsNullOrWhiteSpace(termoPesquisa))
            {
                if (tipoPesquisa == "descricao")
                {
                    cidades = _repositorioCidade.GetByNome(termoPesquisa);
                }
                else if(tipoPesquisa == "uf")
                {
                    cidades = _repositorioCidade.Get(d => d.EnumeradorUF.ToString().Contains(termoPesquisa, System.StringComparison.OrdinalIgnoreCase));
                }
            }

            IEnumerable<CidadeModel> cidadesModel = cidades.Select(c => c.Converta());

            if(!cidadesModel.Any())
            {
                ViewBag.Mensagem = "Nenhuma cidade encontrada para a pesquisa informada.";
            }

            return View("ListaCidade", cidadesModel);
        }
        
        public IActionResult Salvar(int? id)
        {
            if (id is null)
            {
                return View("Cidade", new CidadeModel());
            }

            Cidade? cidade = _repositorioCidade.GetByCodigo(id.Value);
            CidadeModel cidadeModel = cidade.Converta();

            return View("Cidade", cidadeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(CidadeModel cidadeModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Cidade", cidadeModel);
            }

            if (_repositorioCidade.DescricaoExiste(cidadeModel.Descricao, cidadeModel.Codigo))
            {
                ModelState.AddModelError("Descricao", "Já existe uma cidade cadastrada com esse nome.");
                return View("Cidade", cidadeModel);
            }

            Cidade cidade = cidadeModel.Converta();

            if (cidadeModel.Codigo > 0)
            {
                _repositorioCidade.Update(cidade);
            }
            else
            {
                _repositorioCidade.Add(cidade);
            }

            TempData["MensagemSucesso"] = "Cidade salva com sucesso! ✅";
            return RedirectToAction("ListaCidade", "Cidade");
        }

        public IActionResult Remove(int id)
        {
            bool possuiVinculo = _repositorioCidade.PossuiRegistro(id);

            if (possuiVinculo)
            {
                TempData["MensagemErro"] = "A exclusão desta cidade não pode ser realizada, pois há alunos associados a ela.";
                return RedirectToAction("ListaCidade");
            }

            Cidade cidade = _repositorioCidade.GetByCodigo(id);
            _repositorioCidade.Remove(cidade);

            TempData["MensagemSucesso"] = "Cidade excluída com sucesso! ✅";
            return RedirectToAction("ListaCidade");
        }
    }
}
