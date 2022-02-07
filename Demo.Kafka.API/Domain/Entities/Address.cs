namespace Demo.Kafka.API.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    [Owned]
    public class Address : ValueObject
    {
        public string Street1 { get; private set; }
        public string Street2 { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Zip { get; private set; }

        public Address() { }

        public Address(string street1, string street2, string city, string state, string zip)
        {
            Street1 = street1;
            Street2 = street2;
            City = city;
            State = state;
            Zip = zip;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street1;
            yield return Street2;
            yield return City;
            yield return State;
            yield return Zip;
        }

        public override string ToString()
        {
            return $"{Street1}, {Street2}, {City}, {State} {Zip}";
        }
    }
}
