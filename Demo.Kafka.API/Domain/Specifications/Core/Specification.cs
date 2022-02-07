namespace Demo.Kafka.API.Domain.Specifications.Core
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The specification.
    /// </summary>
    /// <typeparam name="T">
    /// The type.
    /// </typeparam>
    public abstract class Specification<T>
    {
        /// <summary>
        /// The to expression.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        /// <summary>
        /// The and.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <returns>
        /// The <see cref="Specification{T}"/>.
        /// </returns>
        public Specification<T> And(Specification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        /// <summary>
        /// The or.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <returns>
        /// The <see cref="Specification{T}"/>.
        /// </returns>
        public Specification<T> Or(Specification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }
    }
}
