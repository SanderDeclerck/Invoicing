using System;

namespace Identity.Service.Domain
{
    public class Tenant
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Tenant(string name)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
        }
    }
}