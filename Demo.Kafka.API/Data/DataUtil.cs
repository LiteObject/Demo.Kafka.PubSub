using Demo.Kafka.API.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Demo.Kafka.API.Data
{
    [ExcludeFromCodeCoverage]
    public static class DataUtil
    {
        public static List<Order> GenerateOrders()
        {
            return new List<Order>
            {
                new()
                {
                    Note = "This is a special order",
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = new Address("123 Street", "House #456", "Frisco", "Texas", "75033"),
                    LineItems = GetLineItems()
                },

                new()
                {
                    Note = "This is a very special order",
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = new Address("456 Street", "House #789", "Dallas", "Texas", "75248"),
                    LineItems = GetLineItems(),
                    Status = Domain.Enums.OrderStatus.ProcessingHalted,
                    Created = DateTime.UtcNow.AddDays(-5),
                    Modified = DateTime.UtcNow,
                }
            };
        }

        private static List<LineItem> GetLineItems()
        {
            return new List<LineItem>
            {
                new LineItem { Name = "Apple", UnitPrice = 0.50, Quantity = 5 },
                new LineItem { Name = "Orange", UnitPrice = 0.65, Quantity = 2 },
                new LineItem { Name = "Mango", UnitPrice = 0.75, Quantity = 5 },
            };
        }
    }
}
