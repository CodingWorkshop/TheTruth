export default (() => {
    return {
        showLoading: () => {
            const target = document.getElementsByClassName(
                'loading-mask-disable'
            );
            if (target.length > 0) {
                target[0].className = 'loading-mask';
            }
        },
        hideLoading: () => {
            const target = document.getElementsByClassName('loading-mask');
            if (target.length > 0) {
                target[0].className = 'loading-mask-disable';
            }
        },
        showLogo: () => {
            const target = document.getElementsByClassName('logo-mask-disable');
            if (target.length > 0) {
                target[0].className = 'logo-mask';
            }
        },
        hideLogo: () => {
            const target = document.getElementsByClassName('logo-mask');
            if (target.length > 0) {
                target[0].className = 'logo-mask-disable';
            }
        }
    };
})();
