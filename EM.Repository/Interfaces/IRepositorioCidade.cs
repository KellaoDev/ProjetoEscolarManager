using EM.Domain;

namespace EM.Repository.Interfaces
{
    public interface IRepositorioCidade : IRepositorioAbstrato<Cidade>
    {
        bool PossuiRegistro(int id);
        Cidade GetByCodigo(int codigo);
        IEnumerable<Cidade> GetByNome(string contendoNome);
        bool DescricaoExiste(string descricao, int? codigoDesconsiderar = null);
    }
}
