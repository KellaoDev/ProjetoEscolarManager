using EM.Domain;
using EM.Repository.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EM.Web.Controllers
{
    public class CidadeController(IRepositorioCidade repositorioCidade) : Controller
    {
        private readonly IRepositorioCidade _repositorioCidade = repositorioCidade;

        public IActionResult ListaCidade()
        {
            IEnumerable<Cidade> listaCidades = _repositorioCidade.GetAll();
            return View(listaCidades);
        }

        public IActionResult Buscar(string termoPesquisa, string tipoPesquisa)
        {
            IEnumerable<Cidade> listaCidades = [];

            if(string.IsNullOrWhiteSpace(tipoPesquisa) || tipoPesquisa == "todos")
            {
                listaCidades = _repositorioCidade.GetAll();
            }
            else if (!string.IsNullOrWhiteSpace(termoPesquisa))
            {
                if (tipoPesquisa == "descricao")
                {
                    listaCidades = _repositorioCidade.Get(d => d.Descricao.Contains(termoPesquisa, System.StringComparison.OrdinalIgnoreCase));
                }
                else if(tipoPesquisa == "uf")
                {
                    listaCidades = _repositorioCidade.Get(d => d.EnumeradorUF.ToString().Contains(termoPesquisa, System.StringComparison.OrdinalIgnoreCase));
                }
            }

            if(!listaCidades.Any())
            {
                ViewBag.Mensagem = "Nenhuma cidade encontrada para a pesquisa informada.";
            }

            return View("ListaCidade", listaCidades);
        }
        
        public IActionResult Salvar(int? id)
        {
            if (id is null)
            {
                ViewBag.isEdicao = false;
                return View("Cidade", new Cidade());
            }
            ViewBag.isEdicao = true;
            Cidade? cidade = _repositorioCidade.Get(d => d.Codigo == id).FirstOrDefault();

            return View("Cidade", cidade);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Salvar(Cidade cidade)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IsEdicao = cidade.Codigo > 0;
                return View("Cidade", cidade);
            }

            if (_repositorioCidade.DescricaoExiste(cidade.Descricao, cidade.Codigo))
            {
                ModelState.AddModelError("Descricao", "Já existe uma cidade cadastrada com esse nome.");
                ViewBag.IsEdicao = cidade.Codigo > 0;
                return View("Cidade", cidade);
            }

            if (_repositorioCidade.CodigoIbgeExiste(cidade.CodigoIBGE, cidade.Codigo))
            {
                ModelState.AddModelError("CodigoIBGE", "Já existe uma cidade com esse Código IBGE.");
                ViewBag.IsEdicao = cidade.Codigo > 0;
                return View("Cidade", cidade);
            }

            if (cidade.Codigo > 0)
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
                TempData["MensagemErro"] = "Não é possível excluir esta cidade pois existem alunos vinculados a ela";
                return RedirectToAction("ListaCidade");
            }

            Cidade cidade = _repositorioCidade.Get(c => c.Codigo == id).First();
            _repositorioCidade.Remove(cidade);

            TempData["MensagemSucesso"] = "Cidade excluída com sucesso! ✅";
            return RedirectToAction("ListaCidade");
        }
    }
}
