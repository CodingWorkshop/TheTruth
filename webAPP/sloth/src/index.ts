import lang from './i18n/zh-TW';
import signalr from './modules/signalr';
import convertor from './modules/convertor';
import http from './modules/http';
import loadingMask from './modules/loadingMask';
import generateVideoImage from './modules/generateVidoeImage';
const defaultVideoImage = generateVideoImage();

loadingMask.showLoading();

export var config: sloth.Config = {};
const defaultConfig: sloth.Config = {
    webApiRoot: 'http://127.0.0.1:5000',
    webApiGetVideoList: '/api/Video/GetVideoList',
    webApiPlayVideo: '/api/Video/PlayVideo',
    signalrApi: '/videohub',
    signalrChannelPlay: 'play',
    defaultPoster: defaultVideoImage,
    defaultType: 'video/mp4',
    defaultName: lang['Video Name'],
    defaultDescription: lang['Video Description']
};

export var player = {} as any;

videojs.addLanguage('zh-tw', lang);
player = videojs('video-learning-player', {
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
