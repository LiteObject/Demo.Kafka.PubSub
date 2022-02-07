namespace Demo.Kafka.API.Data.Repositories
{
    using AutoMapper;    
    using Demo.Kafka.API.Data.Contexts;
    using Demo.Kafka.API.Domain.Entities;

    public class OrderRepository : GenericRepository<Order, OrderDbContext>
    {
        public OrderRepository(OrderDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        /// <inheritdoc />
        public override void Update(Order orderWithUpdatedValues)
        {
            var existingOrder = this.DbSet.Find(orderWithUpdatedValues.Id);

            if (existingOrder != null)
            {
                var entityEntry = Context.Entry(existingOrder);
                entityEntry.CurrentValues.SetValues(orderWithUpdatedValues);
            }

            /***************************************************************************************
             *  SetValues will update properties on the retrieved Order class with properties 
             *  from the updated object (could be DTO) that have matching names.
             *  
             *  Entity Framework also keeps track of the original values when the Order 
             *  object was retrieved and uses those to determine what actually needs to be updated.
             *  
             *  Issue: We cann't (or I don't know how) actually use SetValues to update navigation 
             *  property without assitional custom code.
             ***************************************************************************************/
        }
    }
}
