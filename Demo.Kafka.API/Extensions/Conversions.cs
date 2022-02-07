namespace Demo.Kafka.API.Extensions
{
    public static class Conversions
    {
        public static Domain.Entities.Order ToEntity(this DTO.Order dto)
        {
            // ToDo: Impplement conversion
            return new Domain.Entities.Order { };
        }

        public static DTO.Order ToDto(this Domain.Entities.Order entity)
        {
            // ToDo: Impplement conversion
            return new DTO.Order { };
        }
    }
}
