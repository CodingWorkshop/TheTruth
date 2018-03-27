function covertToPlayList(res: any, config: sloth.Config) {
    return res.map(function(video: any) {
        return {
            name: video.name || '影片名稱',
            description: video.description || '影片描述',
            sources: [
                {
                    src: generateVideoParams(video, config),
                    type: video.type || config.defaultType
                }
            ],
            poster: video.poster || config.defaultPoster,
            thumbnail: [
                {
                    src: video.thumbnail || 'http://via.placeholder.com/121x68'
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
