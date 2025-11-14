using FirebirdSql.Data.FirebirdClient;

namespace EM.Repository.Database
{
    public class DBHelper
    {
        private static readonly string _stringConexao =
            @"Server=localhost; Port=3055; 
                                Database=C:\\WorkKelio\\ProjetoEscolarManager\\EM.Repository\\Database\\BANCO.fdb;
                                User=SYSDBA;
                                Password=masterkey;";

        private static DBHelper? _instancia;

        private DBHelper() { }

        public static DBHelper Instancia => _instancia ??= new DBHelper();

        public static FbConnection CriarConexao()
        {
            FbConnection conn = new(_stringConexao);
            conn.Open();
            return conn;
        }
    }
}
