import { sloth, player } from '../index';
import convertor from './convertor';
import loadingMask from './loadingMask';
export default (data: any[]) => {
    player.playlist([]);
    player.playlist.autoadvance();

    if (data.length === 0) {
        loadingMask.hideLoading();
        loadingMask.showLogo();
        return;
    }

    const list = convertor.covertToPlayList(data, sloth.config);

    player.playlist(list);
    player.playlist.autoadvance(0);
    player.playlist.first();

    loadingMask.hideLoading();
    loadingMask.hideLogo();
};
