using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public class EfEntityRepositoryBase<TEntity, TContex> : IEntityRepository<TEntity>
        where TEntity : BaseEntity, new()
        where TContex : DbContext


    {
        protected readonly TContex Contex;
        protected readonly DbSet<TEntity> DbSet;

        public EfEntityRepositoryBase(TContex context)
        {
            Contex = context;
            DbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity input)
        {
            await DbSet.AddAsync(input);
            await Contex.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity input)
        {
            DbSet.Update(input);
            await Contex.SaveChangesAsync();
        }
        public async Task DeleteAsync(TEntity input)
        {
           DbSet.Remove(input);
           await Contex.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entity = DbSet.Find(id);
            await Contex.SaveChangesAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<ICollection<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate != null)
            {
                return await DbSet.Where(predicate).ToListAsync();
            }
            return await DbSet.ToListAsync(); //bir where yoksa tüm datayı getirir.
        }

 
    }
}
