import defaultConfig from './getDefaultConfig';
import loadingMask from './loadingMask';
import http from './http';
import signalr from './signalr';
import preparePlayList from './preparePlayList';

export default function initail(sloth: sloth.Instance) {
    http.getAppConfig(defaultConfig).then((config: sloth.Config) => {
        loadingMask.showLoading();
        sloth.config = config;
        signalr(sloth.config).then(
            (connection: signalR.HubConnection) => {
                if (!connection) {
                    return;
                }

                http
                    .getVideoList(
                        sloth.config.webApiRoot +
                            sloth.config.webApiGetVideoList
                    )
                    .then(data => {
                        console.log(data);
                        preparePlayList(data);
                    });

                connection.on('playVideo', function playVideoFunc(data: any) {
                    console.log(data);
                    preparePlayList(data);
                });

                connection.on('loginOk', function loginOkFunc(data: any) {
                    console.log(data);
                });

                connection.onclose(() => {
                    setTimeout(initail(sloth), 10000);
                });
            },
            () => {
                setTimeout(initail(sloth), 10000);
            }
        );
    });
}
