import { env } from 'node:process';
import { NodeSDK } from '@opentelemetry/sdk-node';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-grpc';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-grpc';
import { OTLPLogExporter } from '@opentelemetry/exporter-logs-otlp-grpc';
import { SimpleLogRecordProcessor } from '@opentelemetry/sdk-logs';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
  import { DocumentLoadInstrumentation } from '@opentelemetry/instrumentation-document-load';
  import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
  import { UserInteractionInstrumentation } from '@opentelemetry/instrumentation-user-interaction';
  import { XMLHttpRequestInstrumentation } from '@opentelemetry/instrumentation-xml-http-request';

// const otlpServer = env.OTEL_EXPORTER_OTLP_ENDPOINT ?? "http://localhost:16265";

const otlpServer = "http://localhost:16265";

if (otlpServer) {
    console.log(`OTLP endpoint: ${otlpServer}`);

    const sdk = new NodeSDK({
        traceExporter: new OTLPTraceExporter({
            url: `${otlpServer}/v1/traces`,
            headers: {},
        }),
        metricReader: new PeriodicExportingMetricReader({
            exporter: new OTLPMetricExporter({
                url: `${otlpServer}/v1/metrics`
            }),
        }),
        logRecordProcessor: new SimpleLogRecordProcessor({
            exporter: new OTLPLogExporter({
                url: `${otlpServer}/v1/logs`
            })
        }),
        instrumentations: [
     //       new DocumentLoadInstrumentation(),
   //   new UserInteractionInstrumentation(),
   //     new XMLHttpRequestInstrumentation(),
      new FetchInstrumentation(),
  //  new HttpInstrumentation(),
  //          new ExpressInstrumentation(),
        ],
    });

    sdk.start();
} 