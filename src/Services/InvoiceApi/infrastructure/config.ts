const config = {
    cosmosDb: {
        endpoint: process.env.COSMOSDB_ENDPOINT || "",
        key: process.env.COSMOSDB_KEY || ""
    }
};

export default config;