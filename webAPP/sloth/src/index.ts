import langPackage from './i18n/zh-TW';
import signalr from './modules/signalr';
import convertor from './modules/convertor';
import http from './modules/http';
import defaultConfig from './modules/getDefaultConfig';
import loadingMask from './modules/loadingMask';
import generateVideoImage from './modules/generateVidoeImage';
import * as videojs from 'video.js';
import plugin from '../node_modules/videojs-playlist/dist/videojs-playlist.es';
import playlistUi from '../node_modules/videojs-playlist-ui/dist/videojs-playlist-ui.es';
import * as signalR from '../node_modules/@aspnet/signalr/dist/esm/index';

loadingMask.showLoading();
(videojs as sloth.CustomVideoJs).addLanguage('zh-tw', langPackage);

export var sloth: sloth.Instance = {};
export var player = videojs('video-learning-player', {
    language: 'zh-tw'
});

http
    .getAppConfig(defaultConfig)
    .then(config => {
        sloth.config = config;
        signalr(sloth.config);
        return signalr(sloth.config);
    })
    .then((res: any) => {
        return convertor.covertToPlayList(res, sloth.config);
    })
    .then(result => {
        (player as sloth.Player).playlist(result);
        (player as sloth.Player).playlistUi();
        (player as sloth.Player).playlist.autoadvance(0);

        loadingMask.hideLoading();
    });
