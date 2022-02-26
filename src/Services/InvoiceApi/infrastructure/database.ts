import { Container, CosmosClient } from '@azure/cosmos';
import config from './config';
export interface database {
  getInvoicesContainer(): Promise<Container>;
}

export default function createDatabase(): database {
  const client = new CosmosClient({ endpoint: config.cosmosDb.endpoint, key: config.cosmosDb.key });

  const invoiceDatabaseName = "Invoice API";
  const invoiceContainerName = "invoices";

  async function getInvoicesContainer(): Promise<Container> {
    const { database } = await client.databases.createIfNotExists({ id: invoiceDatabaseName });
    const { container } = await database.containers.createIfNotExists({ id: invoiceContainerName, partitionKey: "/id" });
    return container;
  }

  return { getInvoicesContainer };
}