module.exports = {
  '/api': {
    target: process.env['services__IdentityAPI__http__0'],
    pathRewrite: {
      '^/api': '',
    },
  },
};