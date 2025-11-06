using EM.Domain;

namespace EM.Repository.Repositories.Abstractions
{
    public interface IRepositorioAluno : IRepositorioAbstrato<Aluno>
    {
        Aluno GetByMatricula(int matricula);
        IEnumerable<Aluno> GetByContendoNoNome(string contendoNome);
    }
}
