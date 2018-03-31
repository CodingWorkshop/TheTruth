import generateVideoImage from './generateVidoeImage';
const defaultVideoImage = generateVideoImage();

function covertToPlayList(res: any, config: sloth.Config) {
    return res.map(function(video: any) {
        return {
            name: video.name || config.defaultName,
            description: video.description || config.defaultDescription,
            sources: [
                {
                    src: generateVideoParams(video, config),
                    type: video.type || config.defaultType
                }
            ],
            poster: video.poster || config.defaultPoster,
            thumbnail: [
                {
                    src: video.thumbnail || config.defaultPoster
                }
            ]
        };
    });
}

function generateVideoParams(video: any, config: sloth.Config) {
    return (
        config.webApiRoot +
        config.webApiPlayVideo +
        '?' +
        'category=' +
        video.category +
        '&date=' +
        video.date +
        '&code=' +
        video.code
    );
}

export default {
    covertToPlayList: covertToPlayList,
    generateVideoParams: generateVideoParams
};
