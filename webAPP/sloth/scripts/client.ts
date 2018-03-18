import videojs from 'videojs';
const videoLearningPlaySystem =  () =>{
    const player = videojs('video-learning-player');

    function getVideoList() {
        return [{
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
    }
    player['playlist'](getVideoList());
    player['playlistUi']();
    player['playlist'].autoadvance(0);
}

videoLearningPlaySystem();