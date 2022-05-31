using CustomerOnboarding.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Domain.Helpers
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        internal DbContext Context;

        internal DbSet<TEntity> Entity;

        private Hashtable _domainObjects;
        private bool isBatch;
        //public bool _disposed;

        public int ChangeCount;

        public BaseRepository(DbContext context)
        {
            this.Context = context;
            this.Entity = this.Context.Set<TEntity>();
        }

        public void AddEntity<T>(T entity)
        {
            this.Context.Add(entity);
        }

        public EntityEntry GetDbEntityEntry(TEntity entity)
        {
            var dbEntityEntry = this.Context.Entry<TEntity>(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                this.Context.Set<TEntity>().Attach(entity);
            }
            return dbEntityEntry;
        }

        public async Task<bool> SaveAsync()
        {
            ChangeCount = await this.Context.SaveChangesAsync();
            return ChangeCount > 0;
        }

        public void UpdateEntityState(TEntity entity, EntityState entityState)
        {
            var dbEntityEntry = GetDbEntityEntry(entity);
            dbEntityEntry.State = entityState;
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            UpdateEntityState(entity, EntityState.Modified);

            return await SaveAsync();
        }

        public BaseRepository<TValue> Schema<TValue>() where TValue : class
        {
            if (_domainObjects == null)
            {
                _domainObjects = new Hashtable();
            }

            var type = typeof(TValue).Name;
            if (_domainObjects.ContainsKey(type))
            {
                return (BaseRepository<TValue>)_domainObjects[type];
            }

            var repositoryType = typeof(BaseRepository<>);
            _domainObjects.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TValue)), this.Context));
            return (BaseRepository<TValue>)_domainObjects[type];
        }
    }
}
