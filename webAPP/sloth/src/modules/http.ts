function getAppConfig(defaultConfig: sloth.Config) {
    return new Promise((resolve, reject) => {
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
    })
        .then(function(res) {
            return res.json();
        })
        .catch(function(res) {
            console.log(res);
            var fakeList = [];
            for (let index = 0; index < 10; index++) {
                fakeList[index] = {
                    category: 'video-' + index,
                    date: new Date()
                };
            }
            return fakeList;
        });
}

export default {
    getAppConfig: getAppConfig,
    getVideoList: getVideoList
};
