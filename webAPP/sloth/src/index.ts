import lang from './i18n/zh-TW';
import signalr from './modules/signalr';
import convertor from './modules/convertor';

let config: sloth.Config = {};
const defaultConfig: sloth.Config = {
    webApiRoot: 'http://127.0.0.1:5000',
    webApiGetVideoList: '/api/Video/GetVideoList',
    webApiPlayVideo: '/api/Video/PlayVideo',
    signalrApi: '/VideoHub',
    signalrChannelPlay: 'play',
    defaultPoster: 'http://via.placeholder.com/121x68',
    defaultType: 'video/mp4'
};

let player = {} as any;

videojs.addLanguage('zh-tw', lang);
player = videojs('video-learning-player', {
    language: 'zh-tw'
});

getAppConfig()
    .then(function(_config) {
        return _config;
    })
    .then(function(_config) {
        config = _config;
        signalr(config);
        return getVideoList();
    })
    .then((res: any) => {
        return convertor.covertToPlayList(res, config);
    })
    .then(function(result) {
        player.playlist(result);
        player.playlistUi();
        player.playlist.autoadvance(0);
    });

function getAppConfig() {
    return new Promise((resolve, reject) => {
        if (window.location.search) {
            const query = window.location.search.substring(1).split('&');
            const configFromUrl: any = Object.assign({}, defaultConfig);
            query.forEach(q => {
                let keyValuePair = q.split('=');
                configFromUrl[keyValuePair[0]] = keyValuePair[1];
            });
            resolve(configFromUrl);
        } else {
            resolve(defaultConfig);
        }
    });
}

function getVideoList() {
    return fetch(config.webApiRoot + config.webApiGetVideoList, {
        method: 'get'
    })
        .then(function(res) {
            return res.json();
        })
        .catch(function(res) {
            console.log(res);
            var fakeList = [];
            for (let index = 0; index < 10; index++) {
                fakeList[index] = {
                    category: 'video-' + index,
                    date: new Date()
                };
            }
            return fakeList;
        });
}
