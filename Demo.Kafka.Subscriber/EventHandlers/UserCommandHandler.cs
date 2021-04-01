namespace Demo.Kafka.Subscriber.EventHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Demo.Kafka.ClassLibrary.Entities;
    using Demo.Kafka.ClassLibrary.Events;
    using MediatR;

    public class UserCommandHandler: IRequestHandler<UserCreated, User>
    {
        public async Task<User> Handle(UserCreated request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.Email);
            return await Task.FromResult<User>(null);
        }
    }
}
