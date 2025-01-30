using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IEntityRepository<TEntity> 
        where TEntity : BaseEntity,new()
    {
        Task AddAsync(TEntity input);
        Task UpdateAsync(TEntity input);
        Task DeleteAsync(TEntity input);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        // expression ifadesi : db sorgusuna dönüştürülebilr demek. 
        // LINQ expression tree oluşturur.
        // Entity Framework gibi ORM'ler bunu SQL'e dönüştürür
        // predicate : koşuldur.
        Task<ICollection<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task DeleteByIdAsync(Guid id);
    }
}
