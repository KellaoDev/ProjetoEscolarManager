using EM.Domain;
using EM.Domain.Enums;

namespace EM.Repository.Repositories.Abstractions
{
    public interface IRepositorioAluno : IRepositorioAbstrato<Aluno>
    {
        Aluno GetByMatricula(int matricula);
        IEnumerable<Aluno> GetByContendoNoNome(string contendoNome);
        bool CpfExiste(string? cpf, int? matriculaDesconsiderar = null);
    }
}
