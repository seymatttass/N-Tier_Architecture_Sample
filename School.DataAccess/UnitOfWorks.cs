using Core.DataAccess;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess
{
    public class UnitOfWorks : IUnitOfWorks
    {
        private readonly SchoolDbContex _contex;
        public UnitOfWorks(SchoolDbContex contex)
        {
            _contex = contex;
        }

        public IEntityRepository<TEntity> GenerateRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            return new EfEntityRepositoryBase<TEntity, SchoolDbContex>(_contex);
        }

        public async Task BeginTransactionAsync()
        {
            await _contex.Database.BeginTransactionAsync();
        }
    
        public async Task RollbaskTransactionAsync()
        {
            await _contex.Database.RollbackTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            await _contex.Database.CommitTransactionAsync();
        }


    }
}
