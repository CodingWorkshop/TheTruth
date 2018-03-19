var videoLearningPlaySystem = () => {
    var webServer = '/';
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
                var fakeList = [];
                for (let index = 0; index < 10; index++) {
                    fakeList[index] = {
                        category: 'video-' + index,
                        date: new Date()
                    };
                }
                return fakeList;
            });
    }

    function covertToPlayList(res) {
        return res.map(function (video) {
            return {
                name: '影片名稱',
                description: '影片描述',
                sources: [{
                    src: webServer + 'api/Video/PlayVideo?category=' + video.category + '&date=' + video.date,
                    type: defaultType
                }],
                poster: defaultPoster,
                thumbnail: [{
                    src: 'http://via.placeholder.com/121x68'
                }]
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