import { AzureMonitorTraceExporter } from "@azure/monitor-opentelemetry-exporter";
import { credentials, Metadata } from "@grpc/grpc-js";
import { getNodeAutoInstrumentations } from "@opentelemetry/auto-instrumentations-node";
import { OTLPTraceExporter as OTLPGrpcExporter } from "@opentelemetry/exporter-trace-otlp-grpc";
import { OTLPTraceExporter as OTLPHttpExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { registerInstrumentations } from "@opentelemetry/instrumentation";
import { Resource } from "@opentelemetry/resources";
import { BasicTracerProvider, BatchSpanProcessor, ConsoleSpanExporter, SimpleSpanProcessor } from "@opentelemetry/sdk-trace-base";
import { NodeTracerProvider } from "@opentelemetry/sdk-trace-node";
import { SemanticResourceAttributes } from "@opentelemetry/semantic-conventions";

export default function setupTelemetry() {

  const provider = new NodeTracerProvider({
    resource: new Resource({
      [SemanticResourceAttributes.SERVICE_NAME]: 'InvoiceApi',
    })
  });

  registerInstrumentations({
    tracerProvider: provider,
    instrumentations: [getNodeAutoInstrumentations()]
  });

  exportToLightstep(provider);
  exportToHoneycomb(provider);
  exportToApplicationInsights(provider);
  provider.addSpanProcessor(new SimpleSpanProcessor(new ConsoleSpanExporter()));

  provider.register();

  process.on('SIGTERM', () => {
    provider.shutdown()
      .then(() => console.log('Tracing terminated'))
      .catch((error) => console.log('Error terminating tracing', error))
      .finally(() => process.exit(0));
  });
}

function exportToLightstep(provider: BasicTracerProvider) {
  const exporter = new OTLPHttpExporter({
    url: 'https://ingest.lightstep.com/traces/otlp/v0.6',
    headers: {
      "lightstep-access-token": process.env.LIGHTSTEP_ACCESSTOKEN || ''
    }
  });

  provider.addSpanProcessor(new BatchSpanProcessor(exporter));
}

function exportToHoneycomb(provider: BasicTracerProvider) {
  const metadata = new Metadata();
  metadata.set('x-honeycomb-team', process.env.HONEYCOMB_API_KEY || '');
  metadata.set('x-honeycomb-dataset', process.env.HONEYCOMB_DATASET_NAME || '');

  const exporter = new OTLPGrpcExporter({
    url: 'grpc://api.honeycomb.io:443/',
    credentials: credentials.createSsl(),
    metadata
  });
  provider.addSpanProcessor(new BatchSpanProcessor(exporter));
}

function exportToApplicationInsights(provider: BasicTracerProvider) {
  const exporter = new AzureMonitorTraceExporter({
    connectionString: process.env.APPINSIGHTS_CONNECTIONSTRING || ''
  });

  provider.addSpanProcessor(new BatchSpanProcessor(exporter))
}

