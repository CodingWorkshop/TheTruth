import generateVideoImage from './generateVidoeImage';
import langPackage from '../i18n/zh-TW';

const defaultConfig: sloth.Config = {
    webApiRoot: 'http://192.168.0.119:5000',
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
