import { EnvironmentProviders, provideAppInitializer } from "@angular/core";
import { BatchSpanProcessor } from "@opentelemetry/sdk-trace-base";
import { WebTracerProvider } from "@opentelemetry/sdk-trace-web";
import { registerInstrumentations } from "@opentelemetry/instrumentation";
import { OTLPTraceExporter } from "@opentelemetry/exporter-trace-otlp-proto";
import { getWebAutoInstrumentations } from "@opentelemetry/auto-instrumentations-web";
import { resourceFromAttributes } from "@opentelemetry/resources";
import { ATTR_SERVICE_NAME } from "@opentelemetry/semantic-conventions";

export function provideInstrumentation(): EnvironmentProviders {
  return provideAppInitializer(() => {
    const provider = new WebTracerProvider({
      resource: resourceFromAttributes({
        [ATTR_SERVICE_NAME]: "Frontend",
      }),
      spanProcessors: [
        new BatchSpanProcessor(
          new OTLPTraceExporter({
            url: `${globalThis.origin}/v1/traces`,
          }),
        ),
      ],
    });

    provider.register();

    registerInstrumentations({
      instrumentations: [
        getWebAutoInstrumentations({
          "@opentelemetry/instrumentation-document-load": {},
          "@opentelemetry/instrumentation-user-interaction": { enabled: false },
          "@opentelemetry/instrumentation-fetch": {},
          "@opentelemetry/instrumentation-xml-http-request": {},
        }),
      ],
    });
  });
}
