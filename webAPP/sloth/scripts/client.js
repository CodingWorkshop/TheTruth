var videoLearningPlaySystem = () => {
    var player = videojs('video-learning-player');

    function getVideoList() {
        return fetch('/GetVideoList', {
                method: 'get'
        }).then(function (response){
            return response.json();
        });
    }

    getVideoList().then(function(result){
        player.playlist(result);
        player.playlistUi();
        player.playlist.autoadvance(0);
    });
}

videoLearningPlaySystem();