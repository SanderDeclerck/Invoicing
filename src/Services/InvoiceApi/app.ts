import { fastify, FastifyInstance } from "fastify";
import createInvoiceRoute from "./endpoints/createInvoice";
import createDatabaseContext from "./infrastructure/database";

export default async function build(): Promise<FastifyInstance> {
  await setupDatabase();
  const app = await setupFastifyServer();

  return app;
}

async function setupDatabase(): Promise<void> {
  const database = createDatabaseContext();
  await database.createDatabaseAndContainers();
}

async function setupFastifyServer(): Promise<FastifyInstance> {
  const app = fastify();
  app.register(createInvoiceRoute, { prefix: "/v1" });
  return app;
}