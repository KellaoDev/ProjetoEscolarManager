using EM.Domain;
using EM.Domain.Enums;
using EM.Repository.Interfaces;
using EM.Repository.Database;
using EM.Repository.ExtensionMethods;
using System.Data.Common;
using System.Linq.Expressions;

namespace EM.Repository.Repositories
{
    public class RepositorioCidade : IRepositorioAbstrato<Cidade>, IRepositorioCidade
    {
        public void Add(Cidade cidade)
        {
            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText =
                    @"INSERT INTO TBCIDADE (CIDADESCRICAO, CIDAUF, CIDACODIGOIBGE)
                                       VALUES (@CIDADESCRICAO, @CIDAUF, @CIDACODIGOIBGE)";

            cmd.Parameters.CreateParameter("@CIDADESCRICAO", cidade.Descricao);
            cmd.Parameters.CreateParameter("@CIDAUF", cidade.EnumeradorUF);

            cmd.Parameters.CreateParameter("@CIDACODIGOIBGE", cidade.CodigoIBGE > 0 
                    ? cidade.CodigoIBGE
                    : DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public void Remove(Cidade cidade)
        {
            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText = "DELETE FROM TBCIDADE WHERE CIDACODIGO = @CIDACODIGO";

            cmd.Parameters.CreateParameter("@CIDACODIGO", cidade.Codigo);
            cmd.ExecuteNonQuery();
        }

        public void Update(Cidade cidade)
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
            cmd.Parameters.CreateParameter("@CIDAUF", cidade.EnumeradorUF);

            cmd.Parameters.CreateParameter("@CIDACODIGOIBGE", cidade.CodigoIBGE > 0
                ? cidade.CodigoIBGE
                : DBNull.Value);

            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Cidade> GetAll()
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
                    EnumeradorUF = (EnumeradorUF)dr.GetInt32(dr.GetOrdinal("CIDAUF")),
                    CodigoIBGE = dr.IsDBNull(dr.GetOrdinal("CIDACODIGOIBGE"))
                        ? null
                        : dr.GetInt32(dr.GetOrdinal("CIDACODIGOIBGE"))
                };
                cidades.Add(cidade);
            }
            return cidades;
        }

        public Cidade GetByCodigo(int cidadeId)
        {
            Cidade? cidade = GetAll().FirstOrDefault(c => c.Codigo == cidadeId);
            return cidade ?? throw new InvalidOperationException("Não foi possível encontrar a cidade pelo ID");
        }

        public IEnumerable<Cidade> Get(Expression<Func<Cidade, bool>> predicate)
        {
            IEnumerable<Cidade> cidades = GetAll().Where(predicate.Compile());
            return cidades;
        }
        

        public IEnumerable<Cidade> GetByNome(string nomeCidade)
        {
            IEnumerable<Cidade> cidades = GetAll().Where(c => c.Descricao.Contains(nomeCidade, StringComparison.OrdinalIgnoreCase));
            return cidades;
        }
         
        public bool PossuiRegistro(int codigo)
        {
            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM TBALUNO WHERE CIDACODIGO = @CIDACODIGO";
            cmd.Parameters.CreateParameter("@CIDACODIGO", codigo);

            using DbDataReader dr = cmd.ExecuteReader();
            return dr.Read() && dr.GetInt64(0) > 0;
        }

        public bool DescricaoExiste(string? descricao, int? codigoDesconsiderar = null)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                return false;

           
            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText = @"SELECT COUNT(1) 
                                    FROM TBCIDADE
                                    WHERE UPPER(CIDADESCRICAO) = UPPER(@DESCRICAO)
                                    AND (@CODIGO IS NULL OR CIDACODIGO <> @CODIGO)";

            cmd.Parameters.CreateParameter("@DESCRICAO", descricao);
            cmd.Parameters.CreateParameter("@CODIGO", codigoDesconsiderar.HasValue ? codigoDesconsiderar.Value : DBNull.Value);

            using DbDataReader dr = cmd.ExecuteReader();
            return dr.Read() && dr.GetInt64(0) > 0;
        }
            
        public bool CodigoIbgeExiste(int? codigoIbge, int? codigoDesconsiderar = null)
        {
            if (!codigoIbge.HasValue)
                return false;

            
            using DbConnection cn = DBHelper.CriarConexao();
            using DbCommand cmd = cn.CreateCommand();

            cmd.CommandText = @"SELECT COUNT(1)
                                    FROM TBCIDADE
                                    WHERE CIDACODIGOIBGE = @CIDACODIGOIBGE
                                    AND (@CODIGO IS NULL OR CIDACODIGO <> @CODIGO)";

            cmd.Parameters.CreateParameter("@CIDACODIGOIBGE", codigoIbge.Value);
            cmd.Parameters.CreateParameter("@CODIGO", codigoDesconsiderar.HasValue ? codigoDesconsiderar.Value : DBNull.Value);

            using DbDataReader dr = cmd.ExecuteReader();
            return dr.Read() && dr.GetInt64(0) > 0;
        } 
    }
}
