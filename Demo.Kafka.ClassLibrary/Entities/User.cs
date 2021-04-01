namespace Demo.Kafka.ClassLibrary.Entities
{
    using System;

    public  class User
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
