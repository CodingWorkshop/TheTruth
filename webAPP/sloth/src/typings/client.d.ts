declare var videojs: any;
declare var signalR: any;

declare namespace sloth {
    interface Config {
        webApiRoot?: string;
        webApiGetVideoList?: string;
        webApiPlayVideo?: string;
        signalrApi?: string;
        signalrChannelPlay?: string;
        defaultPoster?: string;
        defaultType?: string;
    }
}
