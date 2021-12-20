
function routes({ listInvoices, getInvoiceById }) {

  return [
    {
      method: 'GET',
      url: '/invoices',
      handler: listInvoices
    },
    {
      method: 'GET',
      url: '/invoices/:id',
      handler: getInvoiceById
    }
  ]

}

export default routes;
