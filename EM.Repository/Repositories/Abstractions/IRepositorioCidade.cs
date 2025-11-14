using EM.Domain;

namespace EM.Repository.Repositories.Abstractions
{
    public interface IRepositorioCidade : IRepositorioAbstrato<Cidade>
    {
        bool PossuiRegistro(int id);
        bool DescricaoExiste(string descricao, int? codigoDesconsiderar = null);
        bool CodigoIbgeExiste(int? codigoIbge, int? codigoDesconsiderar = null);
    }
}
