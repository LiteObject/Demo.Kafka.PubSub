namespace Demo.Kafka.API.Domain.Services
{
    public class NewYearDiscountService : IDiscountService
    {
        public double Apply(double price)
        {
            var startDate = new DateTime(DateTime.Today.Year, 12, 1);
            var endDate = new DateTime(DateTime.Today.Year + 1, 1, 31);

            if (DateTime.Today > startDate && DateTime.Today < endDate)
            {
                return (price * 50) / 100;
            }

            return price;
        }
    }
}
