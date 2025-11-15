namespace EM.Domain.Interfaces
{
    public interface IRelatorio<T> where T : class
    {
        byte[] Emita(T parametros);
    }
}
