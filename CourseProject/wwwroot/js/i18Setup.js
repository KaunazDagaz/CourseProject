document.addEventListener('DOMContentLoaded', function () {
    var currentLang = localStorage.getItem('preferredLanguage') || 'en';
    i18next
        .use(i18nextHttpBackend)
        .init({
            lng: currentLang,
            fallbackLng: 'en',
            backend: {
                loadPath: '/locales/{{lng}}/translation.json'
            }
        }, function () {
            localizePage();
        });
});