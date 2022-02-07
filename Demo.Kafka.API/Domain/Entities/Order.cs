namespace Demo.Kafka.API.Domain.Entities
{
    using Demo.Kafka.API.Domain.Enums;
    using Humanizer;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The Order class - Aggregate Root
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// Gets or sets Note.
        /// </summary>
        public string Note { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Created;

        /// <summary>
        /// Gets or sets LineItems (OrderItems)
        /// </summary>
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

        [Required]
        public Address ShippingAddress { get; set; }

        public double CalculateTotal()
        {
            double total = 0;

            foreach (var item in LineItems)
            {
                total += item.CalculateSubTotal();
            }

            return total;
        }

        /// <summary>
        /// Use this method to get a humanized version of the status.
        /// </summary>
        /// <returns>Returns status string</returns>
        public string GetStatus()
        {
            return this.Status.Humanize();
        }

        public string GetOrderDetails()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"\nOrder (Id: {Id}) has been set to {Status} ({GetStatus()})");
            sb.AppendLine($"Shipping info: {ShippingAddress}\n");

            foreach (var line in LineItems)
            {
                sb.AppendLine($"- {line.Name}: Unit Price: ${line.UnitPrice}, Quantity: {line.Quantity}");
            }

            sb.AppendLine($"\nOrder total is ${CalculateTotal()}.");
            return sb.ToString();
        }
    }
}
