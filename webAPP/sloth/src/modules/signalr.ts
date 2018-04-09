import { sloth } from '../index';
import loadingMask from './loadingMask';
import * as signalR from '../../node_modules/@aspnet/signalr/dist/esm/index';

// http://docs.videojs.com/tutorial-player-workflows.html

export default (config: sloth.Config) => {
    return new Promise((resolve, reject) => {
        const connection = new signalR.HubConnection(
            config.webApiRoot + config.signalrApi
        );

        connection
            .start()
            .then(init => {
                console.log('initial');
                resolve(connection);
            })
            .catch(noneApi => {
                console.log('noneApi');
                loadingMask.hideLoading();
                loadingMask.showLogo();
                resolve(null);
            });
    });
};
