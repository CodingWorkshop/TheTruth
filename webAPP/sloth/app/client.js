var videoLearningPlaySystem = () => {
    var webServer = 'http://172.20.10.2:5000/';
    var defaultPoster = 'http://media.w3.org/2010/05/sintel/poster.png';
    var defaultType = 'video/mp4';

    var player = videojs('video-learning-player');

    function getVideoList() {
        return fetch(webServer + 'GetVideoList', { method: 'get' })
            .then(function (res) { return res.json(); })
    }

    function covertToPlayList(res) {
        return res.map(function(video) {
            return {
                sources: [{
                    src: webServer + 'PlayVideo?category=' + video.Category + '&date=' + video.Date,
                    type: defaultType
                }],
                poster: defaultPoster
            }
        });
    }

    getVideoList()
        .then(covertToPlayList(res))
        .then(function (result) {
            player.playlist(result);
            player.playlistUi();
            player.playlist.autoadvance(0);
        });
}

videoLearningPlaySystem();