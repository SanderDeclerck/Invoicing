using System;

namespace Customers.Api.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
