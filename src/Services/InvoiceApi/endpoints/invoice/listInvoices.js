
import { v4 as uuid } from 'uuid'

export default function listInvoices() {

  return async function handleRequest(req, res) {
    return { id: uuid() }
  }

}
