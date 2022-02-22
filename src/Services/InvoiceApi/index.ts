import build from "./app";

const app = build();

app.listen(process.env.PORT || 3000);



// import { Container, CosmosClient } from '@azure/cosmos';
// interface database {
//     getInvoicesContainer(): Promise<Container>;
// }

// async function database(cosmosEndpoint: string, cosmosKey: string): Promise<database> {
//     const client = new CosmosClient({ endpoint: cosmosEndpoint, key: cosmosKey });

//     const { database } = await client.databases.createIfNotExists({ id: "Invoice API" });

//     async function getInvoicesContainer(): Promise<Container> {
//         const { container } = await database.containers.createIfNotExists({ id: "invoices" })
//         return container;
//     }

//     return { getInvoicesContainer };
// }

// declare global {
//     // eslint-disable-next-line @typescript-eslint/no-namespace
//     namespace NodeJS {
//         interface ProcessEnv {
//             COSMOSDB_ENDPOINT: string;
//             COSMOSDB_KEY: string;
//         }
//     }
// }