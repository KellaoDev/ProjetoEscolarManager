using EM.Domain.Interfaces;
using System.Linq.Expressions;

namespace EM.Repository.Interfaces
{
    public interface IRepositorioAbstrato<T> where T : IEntidade
    {
        void Add(T entidade);
        void Remove(T entidade);
        void Update(T entidade);
        IEnumerable<T> GetAll();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    }
}
