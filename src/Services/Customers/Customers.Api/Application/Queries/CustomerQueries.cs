using System;
using System.Collections.Generic;
using System.Linq;
using Customers.Api.Application.Exceptions;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Customers.Api.Application.Queries
{
    public class CustomerQueries : ICustomerQueries
    {
        private readonly string _connectionString;

        public CustomerQueries(string connectionString)
        {
            _connectionString = connectionString;
        }

        public CustomerViewModel GetCustomerById(int customerId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var result = connection.QueryFirstOrDefault(@"select * from Customer where Id = @customerId", new {customerId});

                if (result == null)
                {
                    throw new EntityNotFoundException(customerId);
                }
                
                return MapFullResult(result);
            }
        }

        public List<CustomerViewModelSlim> GetAllCustomers()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var result = connection.Query(@"select * from Customer");

                return result.Select(MapSlimResult).ToList();
            }
        }

        private CustomerViewModel MapFullResult(dynamic result)
        {
            var model = new CustomerViewModel
            {
                Id = (int)result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                CompanyName = result.CompanyName,
                VatNumber = result.VatNumber,
                PhoneNumber = result.PhoneNumber,
                EmailAddress = result.EmailAddress,
                ShippingAddress = new AddressViewModel
                {
                    Street = result.ShippingAddress_Street,
                    City = result.ShippingAddress_City,
                    PostalCode = result.ShippingAddress_PostalCode,
                    IsoCountryCode = result.ShippingAddress_IsoCountryCode
                },
                BillingAddress = new AddressViewModel
                {
                    Street = result.BillingAddress_Street,
                    City = result.BillingAddress_City,
                    PostalCode = result.BillingAddress_PostalCode,
                    IsoCountryCode = result.BillingAddress_IsoCountryCode
                }
            };

            return model;
        }

        private CustomerViewModelSlim MapSlimResult(dynamic result)
        {
            var model = new CustomerViewModelSlim
            {
                Id = (int)result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                CompanyName = result.CompanyName,
                PhoneNumber = result.PhoneNumber,
                EmailAddress = result.EmailAddress
            };

            return model;
        }
    }
}
