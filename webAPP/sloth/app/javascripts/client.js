var videoLearningPlaySystem = function () {
    var config = {};

    var player = videojs('video-learning-player');

    function getAppConfig() {
        return fetch('/app/app.config.json')
            .then(function (res) {
                return res.json();
            });
    }

    function getVideoList() {
        return fetch(config.webServer + 'api/Video/GetVideoList', {
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
                name: video.name || '影片名稱',
                description: video.description || '影片描述',
                sources: [{
                    src: generateVideoParams(video),
                    type: video.type || config.defaultType
                }],
                poster: video.poster || config.defaultPoster,
                thumbnail: [{
                    src: video.thumbnail || 'http://via.placeholder.com/121x68'
                }]
            };
        });;
    }

    function generateVideoParams(video) {
        var path = config.webServer + 'api/Video/PlayVideo?' +
            'category=' +
            video.category +
            '&date=' +
            video.date +
            '&code=' +
            video.code;
        return path;
    }

    function signalrInitail() {
        try {
            var connection = new signalR.HubConnection(config.webServer + '/api/signalr', {
                protocol: new signalR.protocol.msgpack.MessagePackHubProtocol()
            });
        } catch (error) {
            console.log(error);
            return;
        }

        connection.on('send', function (data) {
            console.log(data);
        });

        connection.start()
            .then(function () {
                connection.invoke('send', 'Hello');
            });
    }
    getAppConfig()
        .then(function (_config) {
            config = _config;
            signalrInitail();
            return getVideoList();
        })
        .then(covertToPlayList)
        .then(function (result) {
            player.playlist(result);
            player.playlistUi();
            player.playlist.autoadvance(0);
        });
}

videoLearningPlaySystem();