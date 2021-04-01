using System.ComponentModel.DataAnnotations;
using Demo.Kafka.ClassLibrary.Entities;
using MediatR;

namespace Demo.Kafka.ClassLibrary.Commands
{
    public class CreateUserCommand : IRequest<User>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
