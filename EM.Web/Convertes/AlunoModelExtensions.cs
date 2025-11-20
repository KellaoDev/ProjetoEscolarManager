using EM.Domain;
using EM.Web.Models;

namespace EM.Web.Convertes
{
    public static class AlunoModelExtensions
    {
        public static Aluno Converta(this AlunoModel aluno) => 
            new()
            {
                Matricula = aluno.Matricula,
                Nome = aluno.Nome,
                Cpf = aluno.Cpf,
                DataNascimento = aluno.DataNascimento,
                EnumeradorSexo = aluno.EnumeradorSexo,
                CidadeId = aluno.CidadeId,
            };

        public static AlunoModel Converta(this Aluno entidade)
        {
            AlunoModel model = new()
            {
                Matricula = entidade.Matricula,
                Nome = entidade.Nome,
                Cpf = entidade.Cpf,
                DataNascimento = entidade.DataNascimento,
                EnumeradorSexo = entidade.EnumeradorSexo,
                CidadeId = entidade.CidadeId
            };

            return model;
        }
    }
}
