using EM.Domain;
using EM.Domain.Enums;
using EM.Repository.Database;
using EM.Repository.ExtensionMethods;
using EM.Repository.Repositories.Abstractions;
using System.Data.Common;
using System.Linq.Expressions;

namespace EM.Repository.Repositories
{
    public class RepositorioAluno : IRepositorioAbstrato<Aluno>, IRepositorioAluno
    {
        public void Add(Aluno aluno)
        {
            try
            {
                using DbConnection cn = DBHelper.CriarConexao();
                using DbCommand cmd = cn.CreateCommand();

                cmd.CommandText =
                    @"INSERT INTO TBALUNO (ALUNMATRICULA, ALUNNOME, ALUNCPF, ALUNDTNASC, ALUNSEXO, CIDACODIGO)
                                      VALUES (@ALUNMATRICULA, @ALUNNOME, @ALUNCPF, @ALUNDTNASC, @ALUNSEXO, @CIDACODIGO)";

                cmd.Parameters.CreateParameter("@ALUNMATRICULA", aluno.Matricula);
                cmd.Parameters.CreateParameter("@ALUNNOME", aluno.Nome);
                cmd.Parameters.CreateParameter("@ALUNCPF", aluno.Cpf);
                cmd.Parameters.CreateParameter("@ALUNDTNASC", aluno.DataNascimento);
                cmd.Parameters.CreateParameter("@ALUNSEXO", aluno.EnumeradorSexo);
                cmd.Parameters.CreateParameter("@CIDACODIGO", aluno.CidadeId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível adicionar o aluno.", ex);
            }
        }

        public void Remove(Aluno aluno)
        {
            try
            {
                using DbConnection cn = DBHelper.CriarConexao();
                using DbCommand cmd = cn.CreateCommand();

                cmd.CommandText = "DELETE FROM TBALUNO WHERE ALUNMATRICULA = @ALUNMATRICULA";

                cmd.Parameters.CreateParameter("@ALUNMATRICULA", aluno.Matricula);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Nao foi possível remover o aluno.", ex);
            }
        }

        public void Update(Aluno aluno)
        {
            try
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
                cmd.Parameters.CreateParameter("@ALUNCPF", aluno.Cpf);
                cmd.Parameters.CreateParameter("@ALUNDTNASC", aluno.DataNascimento);
                cmd.Parameters.CreateParameter("@ALUNSEXO", aluno.EnumeradorSexo);
                cmd.Parameters.CreateParameter("@CIDACODIGO", aluno.CidadeId);

                cmd.ExecuteNonQuery();
                tran.Commit();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível atualizar o aluno.", ex);
            }
        }

        public IEnumerable<Aluno> GetAll()
        {
            try
            {
                List<Aluno> alunos = [];

                using DbConnection cn = DBHelper.CriarConexao();
                using DbCommand cmd = cn.CreateCommand();

                cmd.CommandText = @"SELECT ALUNMATRICULA, ALUNNOME, ALUNCPF, ALUNDTNASC, ALUNSEXO,
                                       TBALUNO.CIDACODIGO, CIDADESCRICAO, CIDAUF, CIDACODIGOIBGE
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
                        CidadeId = dr.GetInt32(dr.GetOrdinal("CIDACODIGO")),
                        Cidade = new Cidade
                        {
                            Codigo = dr.GetInt32(dr.GetOrdinal("CIDACODIGO")),
                            Descricao = dr.GetString(dr.GetOrdinal("CIDADESCRICAO")),
                            UF = dr.GetString(dr.GetOrdinal("CIDAUF")),
                            CodigoIBGE = dr.GetInt32(dr.GetOrdinal("CIDACODIGOIBGE"))
                        }
                    };

                    alunos.Add(aluno);
                }
                return alunos;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível todos os alunos", ex);
            }
        }

        public IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            try
            {
                IEnumerable<Aluno> alunos = GetAll().Where(predicate.Compile());
                return alunos;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter os alunos por predicado", ex);
            }
        }

        public IEnumerable<Aluno> GetByContendoNoNome(string contendoNome)
        {
            try
            {
                IEnumerable<Aluno> alunos = GetAll().Where(a => a.Nome.Contains(contendoNome, StringComparison.OrdinalIgnoreCase));
                return alunos;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter os alunos por parte do nome", ex);
            }
        }

        public Aluno GetByMatricula(int matricula)
        {
            try
            {
                Aluno? aluno = GetAll().FirstOrDefault(a => a.Matricula == matricula);
                return aluno ?? throw new InvalidOperationException("Não foi possível encontrar o aluno por matrícula");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter o aluno por matrícula", ex);
            }
        }

        public bool PossuiRegistro(int cidadeId)
        {
            try
            {
                using DbConnection conexao = DBHelper.CriarConexao();
                using DbCommand comando = conexao.CreateCommand();

                comando.CommandText = "SELECT COUNT(*) FROM TBALUNO WHERE CIDACODIGO = @CIDACODIGO";
                comando.Parameters.CreateParameter("@CIDACODIGO", cidadeId);

                using DbDataReader dr = comando.ExecuteReader();
                return dr.Read() && dr.GetInt64(0) > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter registros por aluno", ex);
            }
        }
    }
}
