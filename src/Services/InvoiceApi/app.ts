import "dotenv/config";
import { fastify, FastifyInstance } from "fastify";

import createInvoiceRoute from "./endpoints/createInvoice";

export default function build(): FastifyInstance {
  const app = fastify();

  app.register(createInvoiceRoute, { prefix: "/v1" });

  return app;
}