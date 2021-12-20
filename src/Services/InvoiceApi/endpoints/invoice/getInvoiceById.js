

export default function getInvoiceById() {

  return async function handleRequest(req, res) {
    var id = req.params.id;
    return { id: id };
  }

}