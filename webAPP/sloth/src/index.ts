import langPackage from './i18n/zh-TW';
import initial from './modules/initial';
import signalr from './modules/signalr';
import convertor from './modules/convertor';
import http from './modules/http';
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

initial(sloth);
