function localizeValidationErrors() {
    $('span.text-danger, span.field-validation-error').each(function () {
        const $el = $(this);
        const errorKey = $el.text().trim();
        if (errorKey.startsWith('Error.') && i18next.exists(errorKey)) {
            $el.text(i18next.t(errorKey));
        }
    });
}

$(function () {
    localizeValidationErrors();
    const observerConfig = {
        childList: true,
        characterData: true,
        subtree: true
    };
    const observer = new MutationObserver(function (mutations) {
        localizeValidationErrors();
    });
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        observer.observe(form, observerConfig);
    });
    $('form').on('submit', function () {
        $(document).ajaxComplete(function () {
            localizeValidationErrors();
        });
    });
});