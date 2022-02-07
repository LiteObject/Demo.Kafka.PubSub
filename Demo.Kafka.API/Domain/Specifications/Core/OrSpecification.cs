namespace Demo.Kafka.API.Domain.Specifications.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;

    /// <summary>
    /// The or specification.
    /// </summary>
    /// <typeparam name="T">
    /// The type.
    /// </typeparam>
    public sealed class OrSpecification<T> : Specification<T>
    {
        /// <summary>
        /// The left.
        /// </summary>
        private readonly Specification<T> _left;

        /// <summary>
        /// The right.
        /// </summary>
        private readonly Specification<T> _right;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }

        /// <summary>
        /// The to expression.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody ?? throw new InvalidOperationException($"{nameof(exprBody)} cannot be null."), paramExpr);

            return finalExpr;
        }
    }
}
