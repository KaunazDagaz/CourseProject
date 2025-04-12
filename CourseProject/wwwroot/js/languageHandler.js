function changeLanguage() {
    var selectedLang = document.getElementById('cultureSelect').value;
    localStorage.setItem('preferredLanguage', selectedLang);
    i18next.changeLanguage(selectedLang, function () {
        localizePage();
    });
}

function initializeLanguageSelector() {
    var lang = localStorage.getItem('preferredLanguage') || 'en';
    var selectElement = document.getElementById('cultureSelect');
    if (selectElement) {
        selectElement.value = lang;
        selectElement.addEventListener('change', changeLanguage);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    initializeLanguageSelector();
});