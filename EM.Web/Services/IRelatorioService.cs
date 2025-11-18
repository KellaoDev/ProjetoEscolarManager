namespace EM.Web.Services;

public interface IRelatorioService<T> where T : class
{
    byte[] Emita(T parametros);
}
