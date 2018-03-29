import { player } from '../index';

// http://docs.videojs.com/tutorial-player-workflows.html

export default (config: sloth.Config) => {
    var connection = new signalR.HubConnection(
        config.webApiRoot + config.signalrApi
    );
    connection.start().then(() => {
        connection.invoke('requestVideo');
    });
    connection.on('playVideo', function(data: any) {
        console.log(data);
    });
};
