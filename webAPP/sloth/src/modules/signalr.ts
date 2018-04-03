import { sloth } from '../index';
import * as signalR from '../../node_modules/@aspnet/signalr/dist/esm/index';

// http://docs.videojs.com/tutorial-player-workflows.html

export default (config: sloth.Config) => {
    return new Promise((resolve, reject) => {
        const connection = new signalR.HubConnection(
            config.webApiRoot + config.signalrApi
        );
        connection.start().then(init => {
            resolve(connection);
        });
    });
};
