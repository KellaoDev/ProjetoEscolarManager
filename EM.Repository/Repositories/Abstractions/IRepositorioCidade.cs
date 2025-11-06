using EM.Domain;

namespace EM.Repository.Repositories.Abstractions
{
    public interface IRepositorioCidade : IRepositorioAbstrato<Cidade>
    {
        bool PossuiRegistro(int id);
    }
}
