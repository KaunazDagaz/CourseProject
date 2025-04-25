function localizePage() {
    document.querySelectorAll('[data-i18n]').forEach(elem => {
        const key = elem.getAttribute('data-i18n');
        let options = {};
        if (elem.hasAttribute('data-i18n-options')) {
            options = JSON.parse(elem.getAttribute('data-i18n-options'));
        }
        const translated = i18next.t(key, options);
        elem.textContent = translated;
    });

    document.querySelectorAll('[data-i18n-title]').forEach(elem => {
        const key = elem.getAttribute('data-i18n-title');
        let options = {};
        if (elem.hasAttribute('data-i18n-options')) {
            options = JSON.parse(elem.getAttribute('data-i18n-options'));
        }
        const translated = i18next.t(key, options);
        elem.setAttribute('title', translated);
    });

    document.querySelectorAll('[data-i18n-placeholder]').forEach(elem => {
        const key = elem.getAttribute('data-i18n-placeholder');
        let options = {};
        if (elem.hasAttribute('data-i18n-options')) {
            options = JSON.parse(elem.getAttribute('data-i18n-options'));
        }
        const translated = i18next.t(key, options);
        elem.setAttribute('placeholder', translated);
    });

}

window.localizePage = localizePage;