using Base.Infrastructure;
using Invoicing.Base.Ddd;
using Invoicing.Customers.Domain.CustomerAggregate;
using MongoDB.Bson.Serialization;

namespace Invoicing.Customers.Infrastructure.Data
{
    public class CustomerCollectionConfigurator
    {
        public void Setup()
        {

            BsonClassMap.RegisterClassMap<Entity>(classMapInitializer =>
            {
                classMapInitializer.MapProperty(entity => entity.Id);
            });

            BsonClassMap.RegisterClassMap<TenantEntity<Customer>>(classMapInitializer =>
            {
                classMapInitializer.MapProperty(entity => entity.TenantId);
                classMapInitializer.MapProperty(entity => entity.Entity);

                classMapInitializer.MapCreator(entity => new TenantEntity<Customer>(entity.Entity, entity.TenantId));
            });

            BsonClassMap.RegisterClassMap<Customer>(classMapInitializer =>
            {
                classMapInitializer.SetIsRootClass(true);
                classMapInitializer.SetDiscriminatorIsRequired(true);
                classMapInitializer.AddKnownType(typeof(PrivateIndividual));
                classMapInitializer.AddKnownType(typeof(Company));

                classMapInitializer.MapField(customer => customer.PhoneNumber);
                classMapInitializer.MapField(customer => customer.EmailAddress);
                classMapInitializer.MapField(customer => customer.BillingAddress);
            });

            BsonClassMap.RegisterClassMap<PrivateIndividual>(classMapInitializer =>
            {
                classMapInitializer.MapField(privateIndividual => privateIndividual.FirstName);
                classMapInitializer.MapField(privateIndividual => privateIndividual.LastName);

                classMapInitializer.MapCreator(privateIndividual => new PrivateIndividual(privateIndividual.FirstName, privateIndividual.LastName));
            });

            BsonClassMap.RegisterClassMap<Company>(classMapInitializer =>
            {
                classMapInitializer.MapField(company => company.CompanyName);
                classMapInitializer.MapField(company => company.VatNumber);

                classMapInitializer.MapCreator(company => new Company(company.CompanyName, company.VatNumber));
            });

            BsonClassMap.RegisterClassMap<Address>(classMapInitializer =>
            {
                classMapInitializer.MapField(address => address.Street);
                classMapInitializer.MapField(address => address.City);
                classMapInitializer.MapField(address => address.PostalCode);
                classMapInitializer.MapField(address => address.IsoCountryCode);

                classMapInitializer.MapCreator(address => new Address(address.Street, address.City, address.PostalCode, address.IsoCountryCode));
            });
        }
    }
}