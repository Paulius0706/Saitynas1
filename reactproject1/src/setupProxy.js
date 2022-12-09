const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/api/services"
];

module.exports = function (app) {
    app.use(
        '/api',
        createProxyMiddleware({
            target: 'lionfish-app-7v99e.ondigitalocean.app',
            changeOrigin: true,
        })
    );

    
};
