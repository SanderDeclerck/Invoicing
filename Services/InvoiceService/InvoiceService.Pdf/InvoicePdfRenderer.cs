using System.Globalization;
using InvoiceService.Domain.InvoiceIssuers;
using Invoicing.Services.InvoiceService.Invoices.Domain;
using Microsoft.Extensions.Localization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace InvoiceService.Pdf;

public class InvoicePdfRenderer
{
    private readonly CultureInfo _language;
    private readonly IStringLocalizer<InvoicePdfRenderer> _localizer;

    public InvoicePdfRenderer(CultureInfo language, IStringLocalizer<InvoicePdfRenderer> localizer)
    {
        
        _language = language;
        _localizer = localizer;
    }

    public Stream CreatePdf(Invoice invoice, InvoiceIssuer invoiceIssuer)
    {
        using var cultureScope = new CultureScope(_language);

        var pdf = Document.Create(container => {
            container.Page(page => {
                page.Size(PageSizes.A4);
                page.Margin(1.5f, Unit.Centimetre);
                page.DefaultTextStyle(style => style.FontSize(10));
                
                AddHeader(page, invoice, invoiceIssuer);

                page.Content().Column(column => {
                    column.Item().PaddingVertical(10).LineHorizontal(2).LineColor(Colors.Black);

                    CustomerDetails(column.Item(), invoice);

                    column.Item().PaddingVertical(10).LineHorizontal(2).LineColor(Colors.Black);

                    InvoiceItems(column.Item(), invoice);

                    column.Item().PaddingVertical(10).LineHorizontal(2).LineColor(Colors.Black);

                    column.Item().Text(_localizer["Invoice_PaymentConditions", 30, $"{invoiceIssuer.BankAccount.Iban}"]).AlignCenter();
                });
            });
        });

        var stream = new MemoryStream(pdf.GeneratePdf());
        stream.Seek(0, SeekOrigin.Begin);

        return stream;
    }

    private void AddHeader(PageDescriptor page, Invoice invoice, InvoiceIssuer invoiceIssuer)
    {
        page.Header().Column(column => {

            column.Item().Row(row => {
                row.ConstantItem(130).Image(invoiceIssuer.Logo);

                row.RelativeItem(1.2f).PaddingHorizontal(20).Column(column => {
                    column.Item().Text(invoiceIssuer.Name).Bold();

                    column.Item().Text(invoiceIssuer.Address.StreetAndNumber);
                    column.Item().Text($"{invoiceIssuer.Address.PostalCode} {invoiceIssuer.Address.City}");

                    column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten3);

                    column.Item().Text(invoiceIssuer.Email);
                    column.Item().Text(invoiceIssuer.Phone);

                    column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten3);

                    column.Item().Text(invoiceIssuer.VatNumber.Number);
                    column.Item().Text(invoiceIssuer.VatNumber.Registration);

                    column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten3);

                    column.Item().Text($"{invoiceIssuer.BankAccount.Iban} ({invoiceIssuer.BankAccount.Bic})");
                });

                row.RelativeItem(1).Column(column => {
                    column.Item().Text(_localizer["Invoice_Title"]).Bold().FontSize(20);
                    
                    column.Item().PaddingTop(4).PaddingBottom(8).LineHorizontal(2).LineColor(Colors.Black);

                    column.Item().Text(text => {
                        text.Span($"{_localizer["Invoice_Number"]}: ").Bold();
                        text.Span($"{invoice.InvoiceNumber}");
                    });

                    column.Item().Text(text => {
                        text.Span($"{_localizer["Invoice_Date"]}: ").Bold();
                        text.Span($"{invoice.InvoiceDate:yyyy-MM-dd}");
                    });
                });
            });
        });
        
    }

    private void CustomerDetails(IContainer container, Invoice invoice)
    {
        container.Column(column => {
            column.Item().Text(invoice.Customer.Name).Bold();

            column.Item().PaddingTop(5).PaddingBottom(5).LineHorizontal(2).LineColor(Colors.Grey.Lighten3);

            column.Item().Text(invoice.Customer.Address.StreetAndNumber);
            column.Item().Text(text => {
                text.Span(invoice.Customer.Address.PostalCode);
                text.Span(" ");
                text.Span(invoice.Customer.Address.City);
            });
            column.Item().Text(invoice.Customer.Address.Country);
            column.Item().Text(invoice.Customer.VatNumber?.Number ?? "");
        });
    }

    private void InvoiceItems(IContainer container, Invoice invoice)
    {
        container.Table(table => {
            table.ColumnsDefinition(columns => {
                columns.RelativeColumn(3);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
                columns.RelativeColumn(1);
                columns.RelativeColumn(0.7f);
            });

            table.Cell().Padding(2).Text(_localizer["Invoice_Item_Description"]).Bold().AlignCenter();
            table.Cell().Padding(2).Text(_localizer["Invoice_Item_UnitPrice"]).Bold().AlignCenter();
            table.Cell().Padding(2).Text(_localizer["Invoice_Item_Quantity"]).Bold().AlignCenter();
            table.Cell().Padding(2).Text(_localizer["Invoice_Item_TotalPrice"]).Bold().AlignCenter();
            table.Cell().Padding(2).Text(_localizer["Invoice_Item_VatRate"]).Bold().AlignCenter();

            table.Cell().ColumnSpan(5).Padding(2).PaddingVertical(5).LineHorizontal(2).LineColor(Colors.Grey.Lighten3);

            foreach (var item in invoice.InvoiceLines.Items)
            {
                table.Cell().Padding(2).Text(item.Description);
                table.Cell().Padding(2).Text($"{item.UnitPrice:0.00} {invoice.Currency}").AlignRight();
                table.Cell().Padding(2).Text($"{item.Quantity}").AlignCenter();
                table.Cell().Padding(2).Text($"{item.TotalExcludingVat:0.00} {invoice.Currency}").Bold().AlignRight();
                table.Cell().Padding(2).Text($"{item.VatPercentage} %").AlignCenter();
            }
            
            table.Cell().ColumnSpan(5).Padding(2).PaddingVertical(5).LineHorizontal(2).LineColor(Colors.Grey.Lighten3);

            table.Cell().ColumnSpan(3).Padding(2).Text(_localizer["Invoice_TotalExcludingVat"]).AlignRight();
            table.Cell().Padding(2).Text($"{invoice.CalculateTotal().TotalExcludingVat:0.00} {invoice.Currency}").AlignRight();
            table.Cell().Padding(2).Text("");
            
            foreach (var vatTotal in invoice.CalculateTotal().VatTotals)
            {
                table.Cell().ColumnSpan(3).Padding(2).Text($"{_localizer["Invoice_VatRate", vatTotal.VatPercentage]}").AlignRight();
                table.Cell().Padding(2).Text($"{vatTotal.VatAmount:0.00} {invoice.Currency}").AlignRight();
                table.Cell().Padding(2).Text("").AlignRight();
            }

            table.Cell().ColumnSpan(3).Padding(2).Text(_localizer["Invoice_TotalIncludingVat"]).AlignRight().Bold().Underline().FontSize(12);
            table.Cell().Padding(2).Text($"{invoice.CalculateTotal().TotalIncludingVat:0.00} {invoice.Currency}").Bold().Underline().AlignRight().FontSize(12);
            table.Cell().Padding(2).Text("");
        });
    }

    static InvoicePdfRenderer()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }
}
