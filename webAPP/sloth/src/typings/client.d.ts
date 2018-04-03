declare var videojs: any;
declare namespace sloth {
    interface Instance {
        config?: Config;
        action?: Action;
    }

    interface Action {
        initial(): void;
        play(): void;
    }

    interface Config {
        webApiRoot?: string;
        webApiGetVideoList?: string;
        webApiPlayVideo?: string;
        signalrApi?: string;
        signalrChannelPlay?: string;
        defaultPoster?: string;
        defaultType?: string;
        defaultName?: string;
        defaultDescription?: string;
    }

    interface VideoSlice {
        id: string;
        name: string;
        date: string;
        displayName: string;
        code: string;
    }

    interface CustomVideoJs {
        (element: string, object: any): any;
        addLanguage(langauge: string, langPackage: any): void;
        registerPlugin(pluginName: string, plugin: any): void;
        plugin(pluginName: string, plugin: any): void;
    }

    interface Player extends videojs.Player {
        playlist: Playlist;
        playlistUi: PlaylistUi;
    }

    interface Playlist {
        (obj?: any): any;
        autoadvance(num: number): void;
        first(): void;
    }

    interface PlaylistUi {
        (): any;
    }
}
