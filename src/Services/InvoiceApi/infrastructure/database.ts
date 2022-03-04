import { Container, CosmosClient } from '@azure/cosmos';
import config from './config';
export interface database {
  getInvoicesContainer(): Container;
  createDatabaseAndContainers(): Promise<void>;
}

export default function createDatabaseContext(): database {
  const client = new CosmosClient({ endpoint: config.cosmosDb.endpoint, key: config.cosmosDb.key });

  const invoiceDatabaseName = "Invoice API";
  const invoiceContainerName = "invoices";

  async function createDatabaseAndContainers(): Promise<void> {
    const { database } = await client.databases.createIfNotExists({ id: invoiceDatabaseName });
    await database.containers.createIfNotExists({ id: invoiceContainerName, partitionKey: "/id" });
  }

  function getInvoicesContainer(): Container {
    return client.database(invoiceDatabaseName).container(invoiceContainerName);
  }

  return { getInvoicesContainer, createDatabaseAndContainers };
}