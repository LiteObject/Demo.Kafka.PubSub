namespace Demo.Kafka.API.Domain.Entities
{
    /// <summary>
    /// The LineItem/OrderItem - Child Entity
    /// </summary>
    public class LineItem : BaseEntity
    {
        private double _unitPrice;

        public string Name { get; set; }
        public string Description { get; set; }
        public double UnitPrice
        {
            get { return _unitPrice; }
            set
            {
                if (value < 1)
                {
                    throw new InvalidOperationException($"{nameof(value)} must be >= 1");
                }

                this._unitPrice = value;
            }
        }
        public double Quantity { get; set; }

        public double CalculateSubTotal()
        {
            return UnitPrice * Quantity;
        }
    }
}
