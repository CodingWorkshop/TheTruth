var videoLearningPlaySystem = () => {
    var webServer = 'http://192.168.0.105:5001/';
    var defaultPoster = 'http://media.w3.org/2010/05/sintel/poster.png';
    var defaultType = 'video/mp4';

    var player = videojs('video-learning-player');

    function getVideoList() {
        return fetch(webServer + 'api/Video/GetVideoList', {
                method: 'get'
            })
            .then(function (res) {
                return res.json();
            }).catch(function (res) {
                console.log(res);
            });
    }

    function covertToPlayList(res) {
        return res.map(function (video) {
            return {
                sources: [{
                    src: webServer + 'api/Video/PlayVideo?category=' + video.category + '&date=' + video.date,
                    type: defaultType
                }],
                poster: defaultPoster
            };
        });;
    }

    getVideoList()
        .then(covertToPlayList)
        .then(function (result) {
            player.playlist(result);
            player.playlistUi();
            player.playlist.autoadvance(0);
        });
}

videoLearningPlaySystem();