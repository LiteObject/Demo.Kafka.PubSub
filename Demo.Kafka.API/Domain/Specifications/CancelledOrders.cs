namespace Demo.Kafka.API.Domain.Specifications
{
    using Demo.Kafka.API.Domain.Entities;
    using Demo.Kafka.API.Domain.Specifications.Core;
    using System;
    using System.Linq.Expressions;

    public class CancelledOrders : Specification<Order>
    {
        public override Expression<Func<Order, bool>> ToExpression() =>
            order =>
                order.Status == Domain.Enums.OrderStatus.CancelledByBuyer ||
                order.Status == Domain.Enums.OrderStatus.CancelledBySystem;
    }
}
