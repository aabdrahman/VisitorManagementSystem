using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public interface IRepositoryBase<T> where T : class
{
    IQueryable<T> FindAll(bool trackChanges, bool ignoreQueryFilter);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges, bool ignoreQueryFilter);
    Task<IQueryable<T>> ExecuteProcedure(string query, params object[] parameters);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);

}
