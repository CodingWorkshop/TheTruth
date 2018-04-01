declare namespace sloth {
    interface Instance {
        config: Config;
        action: Action;
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

    interface Videojs {
        (): any;
        addLanguage(language: string, languagePackage: object): void;
    }
}
