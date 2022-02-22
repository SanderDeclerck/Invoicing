import { FastifyInstance } from "fastify";

export default function createInvoiceRoute(fastify: FastifyInstance, { }, done: () => void): void {
  fastify.post("/invoice", createInvoice);

  done();
}

interface createInvoiceResponse {
  id: number;
}

async function createInvoice(): Promise<createInvoiceResponse> {
  return { id: 5 };
}

