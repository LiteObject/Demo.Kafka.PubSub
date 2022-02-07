namespace Demo.Kafka.API.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Demo.Kafka.API.Domain.Entities;
    using Demo.Kafka.API.Domain.Specifications.Core;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The generic repository.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <typeparam name="TContext">
    /// The database context type.
    /// </typeparam>
    public class GenericRepository<TEntity, TContext> : IRepository<TEntity>, IDisposable
        where TEntity : BaseEntity
        where TContext : DbContext
    {
        /// <summary>
        /// The database set.
        /// </summary>
        protected readonly DbSet<TEntity> DbSet;

        /// <summary>
        /// The context.
        /// </summary>
        protected readonly TContext Context;

        /// <summary>
        /// The mapper.
        /// </summary>
        protected readonly IMapper mapper;

        /// <summary>
        /// Has Dispose already been called?
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity,TContext}"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public GenericRepository(TContext context, IMapper mapper)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<TEntity>();
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public virtual async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.DbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<List<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.GetAllIncluding(includeProperties);
            var results = await query.Where(predicate).ToListAsync();
            return results;
        }

        /// <inheritdoc />
        public virtual async Task<List<T1>> FindAsync<T1>(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.GetAllIncluding(includeProperties);
            // return await query.Where(predicate).ProjectTo<T1>(this.mapper.ConfigurationProvider).DecompileAsync().ToListAsync();
            return await query.Where(predicate).ProjectTo<T1>(this.mapper.ConfigurationProvider).ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<List<TEntity>> FindAsync(Specification<TEntity> specification)
        {
            return await this.DbSet.Where(specification.ToExpression()).ToListAsync();
        }

        public virtual async Task<List<TEntity>> FindAsync(
            Specification<TEntity> specification,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.GetAllIncluding(includeProperties);
            return await query.Where(specification.ToExpression()).ToListAsync();
        }

        /// <inheritdoc />
        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.Where(predicate).AsNoTracking().ToList();
        }

        /// <inheritdoc />
        public virtual List<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this.GetAllIncluding(includeProperties);
            var results = query.Where(predicate).AsNoTracking().ToList();
            return results;
        }

        /// <inheritdoc />
        public virtual async Task<List<TEntity>> GetAllAsync(int pageNumber, int pageSize)
        {
            if (pageSize < 1)
            {
                pageSize = 1;
            }

            var skip = (pageNumber - 1) * pageSize;
            var take = pageSize;

            return await this.DbSet.AsNoTracking().Skip(skip).Take(take).ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> GetAsync<TKey>(TKey id)
        {
            // Assuming this method will also be used for updating, so let's remove ".AsNoTracking()"
            return await this.DbSet.SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        /// <inheritdoc />
        public virtual async Task Add(TEntity entity)
        {
            await this.DbSet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual void Update(TEntity entity)
        {
            // This way EF updates everything, whether actual changes exist or not,
            // including navigation props, so the SQL update statement is not efficient.
            this.DbSet.Update(entity);
        }

        /// <inheritdoc />
        public virtual void Delete(TEntity entity)
        {
            this.DbSet.Remove(entity);
        }

        /// <inheritdoc />
        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Console.Error.WriteLine(ex.Message);
                }

                throw;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern
        /// </summary>
        /// <param name="disposing">
        /// The disposing parameter.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.Context.Dispose();
                }
            }

            this.disposed = true;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="includeProperties">
        /// The include properties.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        private IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = this.DbSet.AsNoTracking();
            return includeProperties.Aggregate(queryable, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
