import { database } from "../../infrastructure/database";
import { v4 as uuid } from "uuid";

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

  const container = await deps.database.getInvoicesContainer();
  container.items.create(invoice);

  return invoice;
}
