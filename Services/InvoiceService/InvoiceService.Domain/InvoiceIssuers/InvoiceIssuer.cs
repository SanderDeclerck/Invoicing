using Invoicing.SharedKernel.Domain;

namespace InvoiceService.Domain.InvoiceIssuers;

public class InvoiceIssuer : EntityBase, IAggregateRoot
{
    public InvoiceIssuer(Guid id, string name, IssuerAddress address, VatNumber vatNumber, BankAccount bankAccount, string email, string phone, byte[] logo) : base(id)
    {
        Name = name;
        Address = address;
        VatNumber = vatNumber;
        BankAccount = bankAccount;
        Email = email;
        Phone = phone;
        Logo = logo;
    }

    public string Name { get; private set; }
    public IssuerAddress Address { get; private set; }
    public VatNumber VatNumber { get; private set; }
    public BankAccount BankAccount { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public byte[] Logo { get; private set; }
}
