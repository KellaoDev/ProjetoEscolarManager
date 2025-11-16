using EM.Domain;

namespace EM.Repository.Repositories.Abstractions
{
    public interface IRepositorioCidade : IRepositorioAbstrato<Cidade>
    {
        bool PossuiRegistro(int id);
        Cidade GetByCodigo(int codigo);
        IEnumerable<Cidade> GetByNome(string contendoNome);
        bool DescricaoExiste(string descricao, int? codigoDesconsiderar = null);
        bool CodigoIbgeExiste(int? codigoIbge, int? codigoDesconsiderar = null);
    }
}
