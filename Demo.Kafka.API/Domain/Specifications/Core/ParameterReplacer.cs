namespace Demo.Kafka.API.Domain.Specifications.Core
{
    using System.Linq.Expressions;

    /// <summary>
    /// The parameter replacer.
    /// </summary>
    internal class ParameterReplacer : ExpressionVisitor
    {
        /// <summary>
        /// The parameter.
        /// </summary>
        private readonly ParameterExpression parameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterReplacer"/> class.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        internal ParameterReplacer(ParameterExpression parameter)
        {
            this.parameter = parameter;
        }

        /// <summary>
        /// The visit parameter.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression node)
            => base.VisitParameter(parameter);
    }
}
