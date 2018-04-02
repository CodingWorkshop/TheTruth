export default {
    modalToObjectParser(modal: any) {
        let object: any = {};
        Object.keys(modal).forEach(key => {
            const keyName = key.charAt(0).toLowerCase() + key.slice(1);

            object[keyName] = modal[key];
        });
        return object;
    },

    objectToModalParser(object: any) {
        let modal: any = {};
        Object.keys(object).forEach(key => {
            const keyName = key.charAt(0).toUpperCase() + key.slice(1);

            modal[keyName] = object[key];
        });
        return modal;
    }
};
