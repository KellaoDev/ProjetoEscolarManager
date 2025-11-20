using EM.Domain;
using EM.Web.Models;

namespace EM.Web.Convertes
{
    public static class CidadeModelExtensions
    {
        public static Cidade Converta(this CidadeModel cidade) =>
            new()
            {
                Codigo = cidade.Codigo,
                Descricao = cidade.Descricao,
                EnumeradorUF = cidade.EnumeradorUF
            };

        public static CidadeModel Converta(this Cidade entidade)
        {
            CidadeModel model = new()
            {
                Codigo = entidade.Codigo,
                Descricao = entidade.Descricao,
                EnumeradorUF = entidade.EnumeradorUF
            };

            return model;
        }
    }
}
