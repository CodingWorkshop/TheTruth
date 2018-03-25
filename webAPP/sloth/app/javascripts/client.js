var videoLearningPlaySystem = function () {
    var config = {};
    var defaultConfig = {
        "webServer": "127.0.0.1:3000",
        "signalrApi": "/hubs",
        "defaultPoster": "http://via.placeholder.com/121x68",
        "defaultType": "video/mp4"
    };

    var player = videojs('video-learning-player');

    getAppConfig()
        .then(function (_config) {
            config = _config;
            webSocketInitail();
            return getVideoList();
        })
        .then(covertToPlayList)
        .then(function (result) {
            player.playlist(result);
            player.playlistUi();
            player.playlist.autoadvance(0);
        });

    function getAppConfig() {
        var configPromise = new Promise((resolve, reject) => {
            if (window.location.search) {
                const query = window.location.search.substring(1).split('&');
                const configFromUrl = Object.assign({}, defaultConfig);
                query.forEach(q => {
                    let keyValuePair = q.split('=');
                    configFromUrl[keyValuePair[0]] = keyValuePair[1];
                });
                console.log(configFromUrl);
                resolve(configFromUrl);
            } else {
                resolve(defaultConfig);
            }
        });
        return configPromise;
    }

    function getVideoList() {
        return fetch(config.webServer + '/api/Video/GetVideoList', {
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

    function webSocketInitail() {
        var connection = new signalR.HubConnection(config.webServer + config.signalrApi);

        connection.on('send', function (data) {
            var DisplayMessagesDiv = document.getElementById("DisplayMessages");
            DisplayMessagesDiv.innerHTML += "<br/>" + data;
        });

        connection.start().then(function () {
            return connection.invoke('send', 'Hello');
        });

        function SendMessage() {
            var msg = document.getElementById("txtMessage").value;
            connection.invoke('send', msg);
        }
    }
}

videoLearningPlaySystem();