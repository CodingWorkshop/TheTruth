import langPackage from './i18n/zh-TW';
import signalr from './modules/signalr';
import convertor from './modules/convertor';
import http from './modules/http';
import defaultConfig from './modules/getDefaultConfig';
import loadingMask from './modules/loadingMask';
import generateVideoImage from './modules/generateVidoeImage';
import preparePlayList from './modules/preparePlayList';

import playlist from '../node_modules/videojs-playlist/dist/videojs-playlist.es';
import playlistUi from '../node_modules/videojs-playlist-ui/dist/videojs-playlist-ui.es';
import * as signalR from '../node_modules/@aspnet/signalr/dist/esm/index';

loadingMask.showLoading();
loadingMask.hideLogo();
videojs.addLanguage('zh-tw', langPackage);

export var sloth: sloth.Instance = {};
export var player = videojs('video-learning-player', {
    language: 'zh-tw'
}) as sloth.Player;

player.playlist([]);
player.playlistUi();
player.playlist.autoadvance(0);

initial();

function initial() {
    http.getAppConfig(defaultConfig).then(config => {
        loadingMask.showLoading();
        sloth.config = config;
        signalr(sloth.config).then((connection: any) => {
            if (!connection) {
                return;
            }
            http
                .getVideoList(
                    sloth.config.webApiRoot + sloth.config.webApiGetVideoList
                )
                .then(data => {
                    console.log(data);
                    preparePlayList(data);
                });

            connection.on('playVideo', (data: any) => {
                console.log(data);
                preparePlayList(data);
            });

            connection.on('loginOk', (data: any) => {
                console.log(data);
            });
        });
    });
}
