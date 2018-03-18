var videoLearningPlaySystem = () => {
    var player = videojs('video-learning-player');

    function getVideoList() {
        var fakeList = [{
            sources: [{
                src: 'http://media.w3.org/2010/05/sintel/trailer.mp4',
                type: 'video/mp4'
            }],
            poster: 'http://media.w3.org/2010/05/sintel/poster.png'
        }, {
            sources: [{
                src: 'http://media.w3.org/2010/05/bunny/trailer.mp4',
                type: 'video/mp4'
            }],
            poster: 'http://media.w3.org/2010/05/bunny/poster.png'
        }, {
            sources: [{
                src: 'http://vjs.zencdn.net/v/oceans.mp4',
                type: 'video/mp4'
            }],
            poster: 'http://www.videojs.com/img/poster.jpg'
        }, {
            sources: [{
                src: 'http://media.w3.org/2010/05/bunny/movie.mp4',
                type: 'video/mp4'
            }],
            poster: 'http://media.w3.org/2010/05/bunny/poster.png'
        }, {
            sources: [{
                src: 'http://media.w3.org/2010/05/video/movie_300.mp4',
                type: 'video/mp4'
            }],
            poster: 'http://media.w3.org/2010/05/video/poster.png'
        }];
        return fetch('/GetVideoList', {
            method: 'get'
        }).then(function (response) {
            return response.json();
        });
    }

    function getVideo(category){}

    getVideoList()
        .then(function (result) {
            player.playlist(result);
            player.playlistUi();
            player.playlist.autoadvance(0);
        });
}

videoLearningPlaySystem();