using System;
using Demo.Kafka.ClassLibrary.Entities;
using MediatR;

namespace Demo.Kafka.ClassLibrary.Events
{
    public class UserCreated : IRequest<User>
    {
        private const string TopicName = "test_mh";

        public UserCreated()
        {
            this.Topic = TopicName;
            this.Uuid = Guid.NewGuid();
        }

        public UserCreated(User user)
        {
            this.Topic = TopicName;
            this.Uuid = user.Uuid;
            this.Name = user.Name;
            this.Email = user.Email;
        }

        public string Topic { get; set; }
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
