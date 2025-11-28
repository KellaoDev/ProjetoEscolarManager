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

                Cidade = aluno.Cidade == null
                    ? null
                    : new Cidade
                    {
                        Codigo = aluno.Cidade.Codigo,
                        Descricao = aluno.Cidade.Descricao,
                        EnumeradorUF = aluno.Cidade.EnumeradorUF
                    }
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

                Cidade = entidade.Cidade == null
                    ? null
                    : new CidadeModel
                    {
                        Codigo = entidade.Cidade.Codigo,
                        Descricao = entidade.Cidade.Descricao,
                        EnumeradorUF = entidade.Cidade.EnumeradorUF
                    }
            };

            return model;
        }
    }
}
