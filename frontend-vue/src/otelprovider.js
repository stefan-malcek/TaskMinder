import {
    BatchSpanProcessor,
    WebTracerProvider,
  } from '@opentelemetry/sdk-trace-web';
  import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
  import { registerInstrumentations } from '@opentelemetry/instrumentation';
  import { DocumentLoadInstrumentation } from '@opentelemetry/instrumentation-document-load';
  import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';
  import { UserInteractionInstrumentation } from '@opentelemetry/instrumentation-user-interaction';
  import { XMLHttpRequestInstrumentation } from '@opentelemetry/instrumentation-xml-http-request';
  import { ConsoleSpanExporter, SimpleSpanProcessor } from '@opentelemetry/sdk-trace-base';
  import { ZoneContextManager } from '@opentelemetry/context-zone';
  import { B3Propagator } from '@opentelemetry/propagator-b3';
  
import { getWebAutoInstrumentations } from '@opentelemetry/auto-instrumentations-web';

import {
    CompositePropagator,
    W3CBaggagePropagator,
    W3CTraceContextPropagator,
  } from '@opentelemetry/core';
  import { Resource } from '@opentelemetry/resources';
  import { SemanticResourceAttributes } from '@opentelemetry/semantic-conventions';


// const provider = new WebTracerProvider({
//   resource: Resource.default().merge(new Resource({
//     // Replace with any string to identify this service in your system
//     'service.name': 'Frontend',
//   })),
// });
// const exporter = new ZipkinExporter();
// provider.addSpanProcessor(new SimpleSpanProcessor(exporter));


const collectorOptions = {
    url: 'http://localhost:16265/v1/traces', // url is optional and can be omitted - default is http://localhost:4318/v1/traces
    //headers: {}, // an optional object containing custom headers to be sent with each request
    //concurrencyLimit: 10, // an optional limit on pending requests
  };
  
//   const provider = new WebTracerProvider();
//   const exporter = new OTLPTraceExporter(collectorOptions);
//   provider.addSpanProcessor(new SimpleSpanProcessor(exporter));

// provider.register();

// // Registering instrumentations
// registerInstrumentations({
//     instrumentations: [
//       new DocumentLoadInstrumentation(),
//       new UserInteractionInstrumentation(),
//       new XMLHttpRequestInstrumentation(),
//       new FetchInstrumentation()
//     ],
//   });


// const provider = new WebTracerProvider();

// // Note: For production consider using the "BatchSpanProcessor" to reduce the number of requests
// // to your exporter. Using the SimpleSpanProcessor here as it sends the spans immediately to the
// // exporter without delay
// provider.addSpanProcessor(new SimpleSpanProcessor(new ConsoleSpanExporter()));
// provider.addSpanProcessor(new SimpleSpanProcessor(new OTLPTraceExporter(collectorOptions)));
// provider.register({
//   contextManager: new ZoneContextManager(),
//   propagator: new B3Propagator(),
// });

// registerInstrumentations({
//   instrumentations: [
//     new FetchInstrumentation({
//       propagateTraceHeaderCorsUrls: /.*/,
//       clearTimingResources: true,
//     }),
//     // getWebAutoInstrumentations({
//     //     '@opentelemetry/instrumentation-fetch': {
//     //       propagateTraceHeaderCorsUrls: /.*/,
//     //       clearTimingResources: true,
//     //       applyCustomAttributesOnSpan(span) {
//     //         span.setAttribute('app.synthetic_request', 'false');
//     //       },
//     //     },
//     //   }),
//   ],
// });


const provider = new WebTracerProvider({
    resource: new Resource({
      [SemanticResourceAttributes.SERVICE_NAME]:
        'vue'//process.env.VUE_PUBLIC_OTEL_SERVICE_NAME,
    }),
  });

  provider.addSpanProcessor(new SimpleSpanProcessor(new OTLPTraceExporter(collectorOptions)));

  const contextManager = new ZoneContextManager();

  provider.register({
    contextManager,
    propagator: new CompositePropagator({
      propagators: [
        new W3CBaggagePropagator(),
        new W3CTraceContextPropagator(),
      ],
    }),
  });

  registerInstrumentations({
    tracerProvider: provider,
    instrumentations: [
      getWebAutoInstrumentations({
        '@opentelemetry/instrumentation-fetch': {
          propagateTraceHeaderCorsUrls: /.*/,
          clearTimingResources: true,
          applyCustomAttributesOnSpan(span) {
            span.setAttribute('app.synthetic_request', 'false');
          },
        },
      }),
    ],
  });
