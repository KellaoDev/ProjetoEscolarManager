using FirebirdSql.Data.FirebirdClient;
using System.Data.Common;

namespace EM.Repository.ExtensionMethods
{
    public static class MetodosDeExtensao
    {
        public static void CreateParameter(this DbParameterCollection dbParameter, string parameterName, object value) =>
            dbParameter.Add(new FbParameter(parameterName, value));
    }
}
