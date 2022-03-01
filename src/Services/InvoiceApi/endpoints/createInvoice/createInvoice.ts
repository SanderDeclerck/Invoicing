import { database } from "../../infrastructure/database";
import { v4 as uuid } from "uuid";
import { api } from "@opentelemetry/sdk-node";

interface response {
  id: string;
  customer: {
    name: string
  }
}

export interface request {
  customerName: string
}

interface dependencies {
  database: database;
}

export default async function createInvoice(request: request, deps: dependencies): Promise<response> {

  const invoice = {
    id: uuid(),
    customer: {
      name: request.customerName
    }
  }
  api.trace.getSpan(api.context.active())?.setAttribute("invoice.id", invoice.id);

  const container = deps.database.getInvoicesContainer();
  await container.items.create(invoice);

  return invoice;
}
