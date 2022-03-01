import { credentials, Metadata } from "@grpc/grpc-js";
import { getNodeAutoInstrumentations } from "@opentelemetry/auto-instrumentations-node";
import { OTLPTraceExporter } from "@opentelemetry/exporter-trace-otlp-grpc";
import { Resource } from "@opentelemetry/resources";
import { NodeSDK } from "@opentelemetry/sdk-node";
import { SemanticResourceAttributes } from "@opentelemetry/semantic-conventions";

export default function setupTelemetry() {

  const metadata = new Metadata();
  metadata.set('x-honeycomb-team', process.env.HONEYCOMB_API_KEY || '');
  metadata.set('x-honeycomb-dataset', process.env.HONEYCOMB_DATASET_NAME || '');

  const traceExporter = new OTLPTraceExporter({
    url: 'grpc://api.honeycomb.io:443/',
    credentials: credentials.createSsl(),
    metadata
  });

  const sdk = new NodeSDK({
    resource: new Resource({
      [SemanticResourceAttributes.SERVICE_NAME]: 'Invoice Api',
    }),
    traceExporter,
    instrumentations: [getNodeAutoInstrumentations()],
  });

  sdk.start();

  process.on('SIGTERM', () => {
    sdk.shutdown()
      .then(() => console.log('Tracing terminated'))
      .catch((error) => console.log('Error terminating tracing', error))
      .finally(() => process.exit(0));
  });
}