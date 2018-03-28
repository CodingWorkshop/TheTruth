import { player } from '../index';

// http://docs.videojs.com/tutorial-player-workflows.html

export default (config: sloth.Config) => {
    var connection = new signalR.HubConnection(
        config.webApiRoot + config.signalrApi
    );

    connection.on('play', function(data: any) {
        player.play();
    });

    connection.on('wait', function(data: any) {
        player.pause();
    });

    connection.on('stop', function(data: any) {});

    connection.on('next', function(data: any) {});
};
