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
        }
    };
})();
