function localizePage() {
    document.querySelectorAll('[data-i18n]').forEach(elem => {
        const key = elem.getAttribute('data-i18n');
        const translated = i18next.t(key);
        elem.textContent = translated;
    });

    document.querySelectorAll('[data-i18n-title]').forEach(elem => {
        const key = elem.getAttribute('data-i18n-title');
        const translated = i18next.t(key);
        elem.setAttribute('title', translated);
    });

    document.querySelectorAll('[data-i18n-placeholder]').forEach(elem => {
        const key = elem.getAttribute('data-i18n-placeholder');
        const translated = i18next.t(key);
        elem.setAttribute('placeholder', translated);
    });

}

window.localizePage = localizePage;