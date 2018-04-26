const http = require('http');
const url = require('url');
const fs = require('fs');
const path = require('path');
const port = process.argv[2] || 3000;
const webpack = require('webpack');
const configuration = require('./webpack.config.js');

let hotReloard = true;

function webpackBuild() {
    let compiler = webpack(configuration);
    compiler.apply(new webpack.ProgressPlugin());

    compiler.run(function(err, stats) {
        hotReloard = true;
    });
}

webpackBuild();

http
    .createServer(function(req, res) {
        console.log(`${req.method} ${req.url}`);
        const parsedUrl = url.parse(req.url);
        let pathname = `./client${parsedUrl.pathname}`;
        const mimeType = {
            '.ico': 'image/x-icon',
            '.html': 'text/html',
            '.js': 'text/javascript',
            '.json': 'application/json',
            '.css': 'text/css',
            '.png': 'image/png',
            '.jpg': 'image/jpeg',
            '.wav': 'audio/wav',
            '.mp3': 'audio/mpeg',
            '.svg': 'image/svg+xml',
            '.pdf': 'application/pdf',
            '.doc': 'application/msword',
            '.eot': 'appliaction/vnd.ms-fontobject',
            '.ttf': 'aplication/font-sfnt'
        };

        fs.exists(pathname, function(exist) {
            if (!exist) {
                res.statusCode = 404;
                res.end(`File ${pathname} not found!`);
                return;
            }

            if (fs.statSync(pathname).isDirectory()) {
                pathname += '/index.html';
            }

            fs.readFile(pathname, function(err, data) {
                if (err) {
                    res.statusCode = 500;
                    res.end(`Error getting the file: ${err}.`);
                } else {
                    const ext = path.parse(pathname).ext;
                    res.setHeader(
                        'Content-type',
                        mimeType[ext] || 'text/plain'
                    );
                    res.end(data);
                }
            });
        });
    })
    .listen(parseInt(port));

fs.watch(path.join(process.cwd(), 'src'), (event, filename) => {
    if (!hotReloard) {
        return;
    }
    hotReloard = false;
    webpackBuild();
});

console.log(`Server listening on port ${port}`);
