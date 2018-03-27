const path = require('path');
const webpack = require('webpack');
const configuration = require('./webpack.config.js');

function init() {
    let compiler = webpack(configuration);
    compiler.apply(new webpack.ProgressPlugin());

    compiler.run(function(err, stats) {});
}

init();
