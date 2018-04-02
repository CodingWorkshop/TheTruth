import langPackage from './i18n/zh-TW';
import signalr from './modules/signalr';
import convertor from './modules/convertor';
import http from './modules/http';
import defaultConfig from './modules/getDefaultConfig';
import loadingMask from './modules/loadingMask';
import generateVideoImage from './modules/generateVidoeImage';
import * as videojs from 'video.js';
import playlist from '../node_modules/videojs-playlist/dist/videojs-playlist.es';
import playlistUi from '../node_modules/videojs-playlist-ui/dist/videojs-playlist-ui.es';
import * as signalR from '../node_modules/@aspnet/signalr/dist/esm/index';

loadingMask.showLoading();
(videojs as sloth.CustomVideoJs).addLanguage('zh-tw', langPackage);
var registerPlugin =
    (videojs as sloth.CustomVideoJs).registerPlugin ||
    (videojs as sloth.CustomVideoJs).plugin;

registerPlugin('playlist', playlist);
registerPlugin('playlistUi', playlistUi);

export var sloth: sloth.Instance = {};
export var player = videojs('video-learning-player', {
    language: 'zh-tw'
});

http.getAppConfig(defaultConfig).then(config => {
    loadingMask.showLoading();
    sloth.config = config;
    const connection = signalr(sloth.config);
    connection.on('playVideo', data => {
        let list = convertor.covertToPlayList(data, sloth.config);
        (player as sloth.Player).playlist(list);
        (player as sloth.Player).playlistUi();
        (player as sloth.Player).playlist.first();

        loadingMask.hideLoading();
    });
});
