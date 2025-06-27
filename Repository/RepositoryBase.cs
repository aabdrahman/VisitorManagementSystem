using Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly RepositoryContext _context;

    protected RepositoryBase(RepositoryContext context)
    {
        _context = context;
    }
    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public IQueryable<T> FindAll(bool trackChanges, bool ignoreQueryFilter)
    {
        IQueryable<T> entities = _context.Set<T>();

        if (!trackChanges)
            entities = entities.AsNoTracking();

        if(ignoreQueryFilter)
            entities = entities.IgnoreQueryFilters();

        return entities;

    }

    public IQueryable<T> FindByCondition(System.Linq.Expressions.Expression<Func<T, bool>> expression, bool trackChanges, bool ignoreQueryFilter)
    {
        IQueryable<T> entities = _context.Set<T>();
        if(!trackChanges)
            entities = entities.AsNoTracking();
        if(ignoreQueryFilter)
            entities = entities.IgnoreQueryFilters();

        return entities.Where(expression);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }
}
