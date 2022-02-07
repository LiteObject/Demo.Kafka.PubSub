using Demo.Kafka.API.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Demo.Kafka.API.DTO
{
    public class Order
    {
        public int Id { get; set; }

        public string Note { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public ICollection<LineItem> LineItems { get; set; } = new List<LineItem>();

        [Required]
        public Address ShippingAddress { get; set; }

        public string Status { get; set; }
    }
}
