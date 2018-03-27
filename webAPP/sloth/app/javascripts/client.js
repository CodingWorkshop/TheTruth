var videoLearningPlaySystem = function () {
    var config = {};
    var defaultConfig = {
        "webApiRoot": "http://127.0.0.1:5000",
        "webApiGetVideoList": "/api/Video/GetVideoList",
        "webApiPlayVideo": "/api/Video/PlayVideo",
        "signalrApi": "/VideoHub",
        "signalrChannelPlay": "play",
        "defaultPoster": "http://via.placeholder.com/121x68",
        "defaultType": "video/mp4"
    };

    var player = {};

    fetch('app/i18n/zh-TW.json')
        .then(function (response) {
            return response.json();
        })
        .then(function (langJson) {
            videojs.addLanguage('zh-tw', langJson);
            player = videojs('video-learning-player', {
                language: 'zh-tw'
            });

            return getAppConfig();
        })
        .then(function (_config) {

            return _config;
        })
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
        return fetch(config.webApiRoot + config.webApiGetVideoList, {
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
        var path = config.webApiRoot + config.webApiPlayVideo + '?' +
            'category=' +
            video.category +
            '&date=' +
            video.date +
            '&code=' +
            video.code;
        return path;
    }

    function webSocketInitail() {
        var connection = new signalR.HubConnection(config.webApiRoot + config.signalrApi);

        connection.on('play', function (data) {
            var DisplayMessagesDiv = document.getElementById("DisplayMessages");
            DisplayMessagesDiv.innerHTML += "<br/>" + data;
        });
    }
}

videoLearningPlaySystem();