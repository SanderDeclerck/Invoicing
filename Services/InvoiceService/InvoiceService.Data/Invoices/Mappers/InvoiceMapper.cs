using InvoiceService.Data.Invoices.Entities;
using Invoicing.Services.InvoiceService.Invoices.Domain;

namespace InvoiceService.Data.Invoices.Mappers;

public static class InvoiceMapper
{
    public static Invoice MapToDomain(this InvoiceEntity invoiceEntity)
    {
        var address = new InvoiceAddress(
            streetAndNumber: invoiceEntity.Customer.Address.StreetAndNumber,
            city: invoiceEntity.Customer.Address.City,
            postalCode: invoiceEntity.Customer.Address.PostalCode,
            country: invoiceEntity.Customer.Address.Country);

        Customer customer;
        if (invoiceEntity.Customer.VatNumber?.Number != null) 
        {
            var vatNumber = new VatNumber(invoiceEntity.Customer.VatNumber.Number);
            customer = new Customer(invoiceEntity.Customer.Name, address, vatNumber);
        }
        else
        {
            customer = new Customer(invoiceEntity.Customer.Name, address);
        }

        var invoice = new Invoice(
            id: invoiceEntity.Id,
            invoiceNumber: invoiceEntity.InvoiceNumber,
            invoiceDate: invoiceEntity.InvoiceDate,
            customer: customer,
            invoiceLines: invoiceEntity.InvoiceLines.Select(x => new InvoiceLine(
                description: x.Description,
                unitPrice: x.UnitPrice,
                quantity: x.Quantity,
                vatPercentage: x.VatPercentage)));

        return invoice;
    }

    public static InvoiceEntity MapToEntity(this Invoice invoice, string tenantId)
    {
        var invoiceEntity = new InvoiceEntity
        {
            Id = invoice.Id,
            TenantId = tenantId,
            InvoiceNumber = invoice.InvoiceNumber,
            InvoiceDate = invoice.InvoiceDate,
            Customer = new CustomerEntity
            {
                Name = invoice.Customer.Name,
                Address = new InvoiceAddressEntity
                {
                    StreetAndNumber = invoice.Customer.Address.StreetAndNumber,
                    City = invoice.Customer.Address.City,
                    PostalCode = invoice.Customer.Address.PostalCode,
                    Country = invoice.Customer.Address.Country
                },
                VatNumber = invoice.Customer.VatNumber != null
                    ? new VatNumberEntity { Number = invoice.Customer.VatNumber.Number }
                    : null
            },
            InvoiceLines = invoice.InvoiceLines.Items.Select(x => new InvoiceLineEntity
            {
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
                VatPercentage = x.VatPercentage
            }).ToList()
        };

        return invoiceEntity;
    }
}