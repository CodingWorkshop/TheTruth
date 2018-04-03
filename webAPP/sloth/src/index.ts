import langPackage from './i18n/zh-TW';
import signalr from './modules/signalr';
import convertor from './modules/convertor';
import http from './modules/http';
import defaultConfig from './modules/getDefaultConfig';
import loadingMask from './modules/loadingMask';
import generateVideoImage from './modules/generateVidoeImage';
import playlist from '../node_modules/videojs-playlist/dist/videojs-playlist.es';
import playlistUi from '../node_modules/videojs-playlist-ui/dist/videojs-playlist-ui.es';
import * as signalR from '../node_modules/@aspnet/signalr/dist/esm/index';

loadingMask.showLoading();
videojs.addLanguage('zh-tw', langPackage);

var player = videojs('video-learning-player', {
    language: 'zh-tw'
}) as sloth.Player;

player.playlist([]);
player.playlistUi();
player.playlist.autoadvance(0);

export var sloth: sloth.Instance = {};

http.getAppConfig(defaultConfig).then(config => {
    loadingMask.showLoading();
    sloth.config = config;
    const connection = signalr(sloth.config);
    connection.on('playVideo', data => {
        const list = convertor.covertToPlayList(data, sloth.config);

        player.playlist(list);
        player.playlist.first();

        loadingMask.hideLoading();
    });
});
