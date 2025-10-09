module.exports = {
  '/api': {
    target: process.env['services__Identity__http__0'],
    pathRewrite: {
      '^/api': '',
    },
  },
  '/v1/traces': {
    target: 'http://localhost:16175',
    headers: parseHeaders(process.env['OTEL_EXPORTER_OTLP_HEADERS']),
  },
};

function parseHeaders(s) {
  const headers = s.split(',');
  const result = {};
 
  headers.forEach((header) => {
    const [key, value] = header.split('=');
    result[key.trim()] = value.trim();
  });
 
  return result;
}