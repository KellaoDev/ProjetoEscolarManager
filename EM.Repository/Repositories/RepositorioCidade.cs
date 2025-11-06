using EM.Domain;
using EM.Repository.Database;
using EM.Repository.ExtensionMethods;
using EM.Repository.Repositories.Abstractions;
using System.Data.Common;
using System.Linq.Expressions;

namespace EM.Repository.Repositories
{
    public class RepositorioCidade : IRepositorioAbstrato<Cidade>, IRepositorioCidade
    {
        public void Add(Cidade cidade)
        {
            try
            {
                using DbConnection cn = DBHelper.CriarConexao();
                using DbCommand cmd = cn.CreateCommand();

                cmd.CommandText =
                    @"INSERT INTO TBCIDADE (CIDACODIGO, CIDADESCRICAO, CIDAUF, CIDACODIGOIBGE)
                                       VALUES (@CIDACODIGO, @CIDADESCRICAO, @CIDAUF, @CIDACODIGOIBGE)";

                cmd.Parameters.CreateParameter("@CIDACODIGO", cidade.Codigo);
                cmd.Parameters.CreateParameter("@CIDADESCRICAO", cidade.Descricao);
                cmd.Parameters.CreateParameter("@CIDAUF", cidade.UF);

                cmd.Parameters.CreateParameter("@CIDACODIGOIBGE", cidade.CodigoIBGE > 0 ?
                    cidade.CodigoIBGE.ToString() :
                    DBNull.Value);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível adicionar a cidade.", ex);
            }
        }

        public void Remove(Cidade cidade)
        {
            try
            {
                using DbConnection cn = DBHelper.CriarConexao();
                using DbCommand cmd = cn.CreateCommand();

                cmd.CommandText = "DELETE FROM TBCIDADE WHERE CIDACODIGO = @CIDACODIGO";

                cmd.Parameters.CreateParameter("@CIDACODIGO", cidade.Codigo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível remover a cidade.", ex);
            }
        }

        public void Update(Cidade cidade)
        {
            try
            {
                using DbConnection cn = DBHelper.CriarConexao();
                using DbCommand cmd = cn.CreateCommand();

                cmd.CommandText = @"UPDATE TBCIDADE SET
                                       CIDADESCRICAO = @CIDADESCRICAO,
                                       CIDAUF = @CIDAUF,
                                       CIDACODIGOIBGE = @CIDACODIGOIBGE
                                       WHERE CIDACODIGO = @CIDACODIGO";

                cmd.Parameters.CreateParameter("@CIDACODIGO", cidade.Codigo);
                cmd.Parameters.CreateParameter("@CIDADESCRICAO", cidade.Descricao);
                cmd.Parameters.CreateParameter("@CIDAUF", cidade.UF);

                cmd.Parameters.CreateParameter("@CIDACODIGOIBGE", cidade.CodigoIBGE > 0 ?
                    cidade.CodigoIBGE.ToString() :
                    DBNull.Value);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível atualizar a cidade.", ex);
            }
        }

        public IEnumerable<Cidade> GetAll()
        {
            try
            {
                List<Cidade> cidades = [];

                using DbConnection cn = DBHelper.CriarConexao();
                using DbCommand cmd = cn.CreateCommand();

                cmd.CommandText = @"SELECT CIDACODIGO, CIDADESCRICAO, CIDAUF, CIDACODIGOIBGE FROM TBCIDADE";

                using DbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Cidade cidade = new()
                    {
                        Codigo = dr.GetInt32(dr.GetOrdinal("CIDACODIGO")),
                        Descricao = dr.GetString(dr.GetOrdinal("CIDADESCRICAO")),
                        UF = dr.GetString(dr.GetOrdinal("CIDAUF")),
                        CodigoIBGE = dr.GetInt32(dr.GetOrdinal("CIDACODIGOIBGE"))
                    };

                    cidades.Add(cidade);
                }
                return cidades;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter todas as cidades.", ex);
            }
        }

        public Cidade GetById(int cidadeId)
        {
            try
            {
                Cidade? cidade = GetAll().FirstOrDefault(c => c.Codigo == cidadeId);
                return cidade ?? throw new InvalidOperationException("Não foi possível encontrar a cidade pelo ID");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter a cidade pelo ID", ex);
            }
        }

        public IEnumerable<Cidade> Get(Expression<Func<Cidade, bool>> predicate)
        {
            try
            {
                IEnumerable<Cidade> cidades = GetAll().Where(predicate.Compile());
                return cidades;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter as cidades por predicado", ex);
            }
        }

        public IEnumerable<Cidade> GetByNome(string nomeCidade)
        {
            try
            {
                IEnumerable<Cidade> cidades = GetAll().Where(c => c.Descricao.Contains(nomeCidade, StringComparison.OrdinalIgnoreCase));
                return cidades;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter as cidades pelo nome", ex);
            }
        }

        //Ajustar
        public bool PossuiRegistro(int codigo)
        {
            try
            {
                using DbConnection conexao = DBHelper.CriarConexao();
                using DbCommand comando = conexao.CreateCommand();

                comando.CommandText = "SELECT COUNT(*) FROM TBALUNO WHERE CIDACODIGO = @CIDACODIGO";
                comando.Parameters.CreateParameter("@CIDACODIGO", codigo);

                using DbDataReader dr = comando.ExecuteReader();
                return dr.Read() && dr.GetInt64(0) > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Não foi possível obter registros por código", ex);
            }
        }
    }
}
