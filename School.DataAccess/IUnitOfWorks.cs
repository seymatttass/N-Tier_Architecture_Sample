using Core.DataAccess;
using Core.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DataAccess
{
    public interface IUnitOfWorks
    {

        IEntityRepository<TEntity> GenerateRepository<TEntity>()
        where TEntity : BaseEntity, new();
        Task BeginTransactionAsync();
        Task RollbaskTransactionAsync();
        Task CommitTransactionAsync();
    }
}
