import { FastifyInstance } from "fastify";
import createDatabaseContext from "../../infrastructure/database";
import createInvoice, { request as createInvoiceRequest } from "./createInvoice";

export default function createInvoiceRoute(fastify: FastifyInstance, { }, done: () => void): void {
  const database = createDatabaseContext();

  fastify.post<{ Body: createInvoiceRequest }>("/invoices", req => createInvoice(req.body, { database }));

  done();
}


