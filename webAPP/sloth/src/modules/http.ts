function getAppConfig(defaultConfig: sloth.Config) {
    return new Promise((resolve, reject) => {
        console.log(window.location.search);
        if (window.location.search) {
            const query = window.location.search.substring(1).split('&');
            const configFromUrl: any = Object.assign({}, defaultConfig);
            query.forEach(q => {
                let keyValuePair = q.split('=');
                configFromUrl[keyValuePair[0]] = keyValuePair[1];
            });
            resolve(configFromUrl);
        } else {
            resolve(defaultConfig);
        }
    });
}

function getVideoList(videoUrl: string) {
    return fetch(videoUrl, {
        method: 'get'
    }).then(res => res.json());
}

export default {
    getAppConfig: getAppConfig,
    getVideoList: getVideoList
};
