import generateVideoImage from './generateVidoeImage';
const defaultVideoImage = generateVideoImage();

function covertToPlayList(videoList: any[], config: sloth.Config) {
    let list = [];

    for (let i = 0; i < videoList.length; i++) {
        list.push({
            name: videoList[i].displayName || config.defaultName,
            description: videoList[i].description || config.defaultDescription,
            sources: [
                {
                    src: generateVideoParams(videoList[i], config),
                    type: config.defaultType
                }
            ],
            poster: videoList[i].poster || config.defaultPoster,
            thumbnail: [
                {
                    src: config.defaultPoster
                }
            ]
        });
    }
    return list;
}

function generateVideoParams(video: any, config: sloth.Config) {
    return (
        config.webApiRoot + config.webApiPlayVideo + '?' + 'code=' + video.code
    );
}

export default {
    covertToPlayList: covertToPlayList,
    generateVideoParams: generateVideoParams
};
