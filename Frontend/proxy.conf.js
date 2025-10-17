export default {
  "/api/auth": {
    target: process.env["services__Identity__http__0"],
    pathRewrite: {
      "^/api/auth": "",
    },
  },
  "/api/main": {
    target: process.env["services__TikalApp__http__0"],
    pathRewrite: {
      "^/api/main": "",
    },
  },
  "/v1/traces": {
    target: "http://localhost:16175",
    headers: parseHeaders(process.env["OTEL_EXPORTER_OTLP_HEADERS"]),
  },
};

function parseHeaders(s) {
  const headers = s.split(",");
  const result = {};

  for (const header of headers) {
    const [key, value] = header.split("=");
    result[key.trim()] = value.trim();
  }

  return result;
}
