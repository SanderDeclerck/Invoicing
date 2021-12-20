import { asFunction } from "awilix";
import routes from "../../endpoints/http/routes.js";
import getInvoiceById from "../../endpoints/invoice/getInvoiceById.js";
import listInvoices from "../../endpoints/invoice/listInvoices.js";

export default function resolveEndpoints(container) {

  // register routes and handlers
  container.register({
    routes: asFunction(routes),
    listInvoices: asFunction(listInvoices),
    getInvoiceById: asFunction(getInvoiceById)
  });

}