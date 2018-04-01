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

export let config: sloth.Config = {};

(videojs as sloth.Videojs).addLanguage('zh-tw', langPackage);

export const player = videojs('video-learning-player', {
    language: 'zh-tw'
});

http.getAppConfig(defaultConfig).then(function(_config) {
    config = _config;
    signalr(config);
    loadingMask.hideLoading();
});
// .then((res: any) => {
//     return convertor.covertToPlayList(res, config);
// })
// .then(function(result) {
//     player.playlist(result);
//     player.playlistUi();
//     player.playlist.autoadvance(0);
// });
