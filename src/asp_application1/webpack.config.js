var path = require('path');

module.exports = {
    entry: "./wwwroot/js/jsx/site.jsx",
    output: {
        path: path.resolve('wwwroot', 'js/'),
        filename: 'site.js'
    },
    resolve: {
        extensions: ['.jsx', '.js']
    },
    module: {
        loaders: [
			{
                loader: 'babel-loader',
                query: {
                    presets: ['react', 'es2015']
                },
                test: /\.jsx$/,
                exclude: /(node_modules|bower_components)/
			}
        ]
    }
};