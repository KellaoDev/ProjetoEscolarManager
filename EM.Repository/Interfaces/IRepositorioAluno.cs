using EM.Domain;

namespace EM.Repository.Interfaces
{
    public interface IRepositorioAluno : IRepositorioAbstrato<Aluno>
    {
        Aluno GetByMatricula(int matricula);
        IEnumerable<Aluno> GetByContendoNoNome(string contendoNome);
        bool CpfExiste(string? cpf, int? matriculaDesconsiderar = null);
    }
}
