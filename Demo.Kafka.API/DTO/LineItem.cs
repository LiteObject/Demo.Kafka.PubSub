using System.ComponentModel.DataAnnotations;

namespace Demo.Kafka.API.DTO
{
    public class LineItem
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double UnitPrice { get; set; }

        [Required]
        public double Quantity { get; set; }
    }
}
