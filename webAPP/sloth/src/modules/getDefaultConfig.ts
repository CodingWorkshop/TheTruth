import generateVideoImage from './generateVidoeImage';
import langPackage from '../i18n/zh-TW';

const defaultConfig: sloth.Config = {
    webApiRoot: 'http://127.0.0.1:5000',
    webApiGetVideoList: '/api/Video/GetVideoList',
    webApiPlayVideo: '/api/Video/PlayVideo',
    signalrApi: '/videohub',
    signalrChannelPlay: 'play',
    defaultPoster: generateVideoImage(),
    defaultType: 'video/mp4',
    defaultName: langPackage['Video Name'],
    defaultDescription: langPackage['Video Description']
};

export default defaultConfig;
