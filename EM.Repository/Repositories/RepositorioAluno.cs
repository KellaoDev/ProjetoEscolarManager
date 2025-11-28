using EM.Domain;
using EM.Domain.Enums;
using EM.Repository.Interfaces;
using EM.Repository.Database;
using EM.Repository.ExtensionMethods;
using System.Data.Common;
using System.Linq.Expressions;

namespace EM.Repository.Repositories
{
    public class RepositorioAluno : RepositorioAbstrato<Aluno>, IRepositorioAluno
    {
        public override void Add(Aluno aluno)
        {
            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText =
                    @"INSERT INTO TBALUNO (ALUNNOME, ALUNCPF, ALUNDTNASC, ALUNSEXO, CIDACODIGO)
                                      VALUES (@ALUNNOME, @ALUNCPF, @ALUNDTNASC, @ALUNSEXO, @CIDACODIGO)";

            cmd.Parameters.CreateParameter("@ALUNNOME", aluno.Nome);
            cmd.Parameters.CreateParameter("@ALUNCPF", aluno.Cpf ?? string.Empty);
            cmd.Parameters.CreateParameter("@ALUNDTNASC", aluno.DataNascimento);
            cmd.Parameters.CreateParameter("@ALUNSEXO", aluno.EnumeradorSexo);
            cmd.Parameters.CreateParameter("@CIDACODIGO", aluno.Cidade?.Codigo ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public override void Remove(Aluno aluno)
        {
            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText = "DELETE FROM TBALUNO WHERE ALUNMATRICULA = @ALUNMATRICULA";

            cmd.Parameters.CreateParameter("@ALUNMATRICULA", aluno.Matricula);
            cmd.ExecuteNonQuery();
        }
        
        public override void Update(Aluno aluno)
        {
            using DbConnection cn = DBHelper.CriarConexao();
            using DbTransaction tran = cn.BeginTransaction();
            using DbCommand cmd = cn.CreateCommand();
            cmd.Transaction = tran;

            cmd.CommandText = @"UPDATE TBALUNO SET
                                       ALUNNOME = @ALUNNOME,
                                       ALUNCPF = @ALUNCPF,
                                       ALUNDTNASC = @ALUNDTNASC,
                                       ALUNSEXO = @ALUNSEXO,
                                       CIDACODIGO = @CIDACODIGO
                                       WHERE ALUNMATRICULA = @ALUNMATRICULA";

            cmd.Parameters.CreateParameter("@ALUNMATRICULA", aluno.Matricula);
            cmd.Parameters.CreateParameter("@ALUNNOME", aluno.Nome);
            cmd.Parameters.CreateParameter("@ALUNCPF", aluno.Cpf ?? string.Empty);
            cmd.Parameters.CreateParameter("@ALUNDTNASC", aluno.DataNascimento);
            cmd.Parameters.CreateParameter("@ALUNSEXO", aluno.EnumeradorSexo);
            cmd.Parameters.CreateParameter("@CIDACODIGO", aluno.Cidade?.Codigo ?? (object)DBNull.Value);

            cmd.ExecuteNonQuery();
            tran.Commit();
        }

        public override IEnumerable<Aluno> GetAll()
        {
            List<Aluno> listaAlunos = [];

            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText = @"SELECT ALUNMATRICULA, ALUNNOME, ALUNCPF, ALUNDTNASC, ALUNSEXO,
                                       TBALUNO.CIDACODIGO, CIDADESCRICAO, CIDAUF
                                       FROM TBALUNO
                                       INNER JOIN TBCIDADE ON TBALUNO.CIDACODIGO = TBCIDADE.CIDACODIGO
                                       ORDER BY ALUNMATRICULA";

            using DbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Aluno aluno = new()
                {
                    Matricula = dr.GetInt32(dr.GetOrdinal("ALUNMATRICULA")),
                    Nome = dr.GetString(dr.GetOrdinal("ALUNNOME")),
                    Cpf = dr.GetString(dr.GetOrdinal("ALUNCPF")),
                    DataNascimento = dr.GetDateTime(dr.GetOrdinal("ALUNDTNASC")),
                    EnumeradorSexo = (EnumeradorSexo)dr.GetInt32(dr.GetOrdinal("ALUNSEXO")),
                    Cidade = new Cidade
                    {
                        Codigo = dr.GetInt32(dr.GetOrdinal("CIDACODIGO")),
                        Descricao = dr.GetString(dr.GetOrdinal("CIDADESCRICAO")),
                        EnumeradorUF = (EnumeradorUF)dr.GetInt32(dr.GetOrdinal("CIDAUF"))
                    }
                };
                listaAlunos.Add(aluno);
            }
            return listaAlunos;
        }

        public override IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            IEnumerable<Aluno> alunos = GetAll().Where(predicate.Compile());
            return alunos;
        }

        public IEnumerable<Aluno> GetByContendoNoNome(string contendoNome)
        {
            IEnumerable<Aluno> alunos = GetAll().Where(a => a.Nome.Contains(contendoNome, StringComparison.OrdinalIgnoreCase));
            return alunos;
        }
        public Aluno GetByMatricula(int matricula)
        {
            Aluno? aluno = GetAll().FirstOrDefault(a => a.Matricula == matricula);
            return aluno ?? throw new InvalidOperationException("Não foi possível encontrar o aluno por matrícula");
        }

        public bool CpfExiste(string? cpf, int? matriculaDesconsiderar = null)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();
                
            cmd.CommandText = @"SELECT COUNT(1) 
                                    FROM TBALUNO
                                    WHERE ALUNCPF = @ALUNCPF
                                    AND (@MATRICULA IS NULL OR ALUNMATRICULA <> @MATRICULA)";
            cmd.Parameters.CreateParameter("@ALUNCPF", cpf);
            cmd.Parameters.CreateParameter("@MATRICULA", matriculaDesconsiderar.HasValue ? matriculaDesconsiderar.Value : DBNull.Value);

            using DbDataReader dr = cmd.ExecuteReader();
            return dr.Read() && dr.GetInt64(0) > 0;
        }
    }
}
