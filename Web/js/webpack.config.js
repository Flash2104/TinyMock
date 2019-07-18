/**
 * Developer: Stepan Burguchev
 * Date: 5/21/2015
 * Copyright: 2009-2016 Comindware®
 *       All Rights Reserved
 * Published under the MIT license
 */

const webpack = require('webpack');
const path = require('path');
const fs = require('fs-extra');
const autoprefixer = require('autoprefixer');
const ExtractTextPlugin = require('extract-text-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const _ = require('lodash');
const cssnano = require('cssnano');
const WebpackPwaManifest = require('webpack-pwa-manifest');
const OfflinePlugin = require('offline-plugin');

const pathResolver = {
    client() {
        //noinspection Eslint
        return path.resolve.apply(path.resolve, [__dirname, '../assets'].concat(_.toArray(arguments)));
    },
    source() {
        //noinspection Eslint
        return path.resolve.apply(path.resolve, [__dirname, '../js/public'].concat(_.toArray(arguments)));
    },
    node_modules() {
        //noinspection Eslint
        return path.resolve.apply(path.resolve, [__dirname, '../node_modules']);
    }
};

const removeBom = text => text.replace(/^\uFEFF/, '');

const readSpritesFile = () => {
    const svgSpritesFile = `${__dirname}/public/styles/sprites.svg`;
    return removeBom(fs.readFileSync(svgSpritesFile, 'utf8'));
};

module.exports = options => {
    const DEVELOPMENT = options.env === 'development';
    const PRODUCTION = options.env === 'production';

    const FONT_LIMIT = PRODUCTION ? 10000 : 1000000;
    const GRAPHICS_LIMIT = PRODUCTION ? 10000 : 1000000;

    //noinspection JSUnresolvedFunction
    const webpackConfig = {
        cache: true,
        entry: {
            app: ['./public/index'],
            vendor: [
                'comindware/core'
            ]
        },
        devtool: 'source-map',
        output: {
            path: pathResolver.client(),
            publicPath: './assets',
            filename: '[name].js',
            sourceMapFilename: '[file].map'
        },
        module: {
            rules: [
                (PRODUCTION ? {
                    test: /\.css$/,
                    loader: ExtractTextPlugin.extract({
                        fallback: 'style-loader',
                        use: [{
                            loader: 'css-loader',
                            options: {
                                sourceMap: true
                            }
                        }, {
                            loader: 'postcss-loader',
                            options: {
                                sourceMap: true,
                                plugins: () => [
                                    autoprefixer({
                                        browsers: ['last 2 versions']
                                    }),
                                    cssnano()
                                ]
                            }
                        }]
                    })
                } : {
                    test: /\.css$/,
                    use: [{
                        loader: 'style-loader'
                    }, {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true
                        }
                    }, {
                        loader: 'postcss-loader',
                        options: {
                            sourceMap: true,
                            plugins: [
                                require('autoprefixer')({
                                    browsers: ['last 2 versions']
                                })
                            ]
                        }
                    }]
                }), {
                    test: /core\.js$/,
                    enforce: 'pre',
                    loader: 'source-map-loader'
                }, {
                    test: /\.js$/,
                    loader: 'babel-loader',
                    include: [
                        pathResolver.source()
                    ],
                    exclude: [
                        pathResolver.source('lib'),
                        pathResolver.source('app/cases')
                    ],
                    options: {
                        presets: ['env'],
                        plugins: ['transform-runtime']
                    }
                }, {
                    test: /\.woff(\?.*)?$/,
                    loader: 'url-loader',
                    options: {
                        prefix: 'fonts/',
                        name: '[path][name].[ext]',
                        limit: FONT_LIMIT,
                        mimetype: 'application/font-woff'
                    }
                }, {
                    test: /\.woff2(\?.*)?$/,
                    loader: 'url-loader',
                    options: {
                        prefix: 'fonts/',
                        name: '[path][name].[ext]',
                        limit: FONT_LIMIT,
                        mimetype: 'application/font-woff2'
                    }
                }, {
                    test: /\.svg(\?.*)?$/,
                    loader: 'url-loader',
                    options: {
                        prefix: 'fonts/',
                        name: '[path][name].[ext]',
                        limit: GRAPHICS_LIMIT,
                        mimetype: 'image/svg+xml'
                    }
                }, {
                    test: /\.(png|jpe?g|gif).*$/,
                    loader: 'url-loader',
                    options: {
                        limit: GRAPHICS_LIMIT
                    }
                }]
        },
        plugins: [
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': PRODUCTION ? '"production"' : '"development"',
                __DEV__: DEVELOPMENT
            }),
            new HtmlWebpackPlugin({
                template: `handlebars-loader!${pathResolver.source('index.hbs')}`,
                hash: PRODUCTION,
                filename: 'index.html',
                svgSprites: readSpritesFile(),
                inject: 'body',
                chunks: ['vendor', 'app'],
                minify: {
                    collapseWhitespace: false
                }
            }),
            new webpack.optimize.CommonsChunkPlugin({
                name: 'vendor',
                minChunks: Infinity
            }),
            new OfflinePlugin(),
            new WebpackPwaManifest({
                name: 'TINYmock',
                short_name: 'Comindware',
                background_color: '#ffffff',
                display: 'standalone',
                theme_color: '#0575bd',
                icons: [{
                    "src": "resources/images/tiny48.png",
                    "sizes": "48x48",
                    "type": "image/png"
                }, {
                    "src": "resources/images/tiny72.png",
                    "sizes": "72x72",
                    "type": "image/png"
                }, {
                    "src": "resources/images/tiny96.png",
                    "sizes": "96x96",
                    "type": "image/png"
                }, {
                    "src": "resources/images/tiny144.png",
                    "sizes": "144x144",
                    "type": "image/png"
                }, {
                    "src": "resources/images/tiny168.png",
                    "sizes": "168x168",
                    "type": "image/png"
                }, {
                    "src": "resources/images/tiny192.png",
                    "sizes": "192x192",
                    "type": "image/png"
                }]
            }),
        ],
        resolve: {
            modules: [
                pathResolver.source(),
                pathResolver.node_modules()
            ],
            alias: {
                'comindware/core': `${pathResolver.node_modules()}/comindware.core.ui/dist/core.js`,
                prism: `${__dirname}/public/lib/prism/prism.js`,
                markdown: `${__dirname}/public/lib/markdown-js/markdown.js`,
                localizationMapEn: `${__dirname}/public/localization/localization.en.json`,
                localizationMapDe: `${__dirname}/public/localization/localization.de.json`,
                localizationMapRu: `${__dirname}/public/localization/localization.ru.json`,
                ajaxMap: `${__dirname}/public/ajax/ajaxMap.js`
            }
        },
        resolveLoader: {
            alias: {
                text: 'html'
            }
        }
    };

    if (PRODUCTION) {
        webpackConfig.cache = false;
        webpackConfig.devtool = false;

        //noinspection JSUnresolvedFunction
        webpackConfig.plugins.push(
            new webpack.DefinePlugin({
                'process.env.NODE_ENV': PRODUCTION ? '"production"' : '"development"',
                __DEV__: DEVELOPMENT
            }),
            new ExtractTextPlugin('[name].css'),
            new webpack.optimize.OccurrenceOrderPlugin(),
            new webpack.optimize.ModuleConcatenationPlugin(),
            new webpack.optimize.UglifyJsPlugin({
                uglifyOptions: {
                    compress: {
                        unused: true,
                        dead_code: true,
                        warnings: false
                    }
                },
                parallel: true
            })
        );
    }

    return webpackConfig;
};
