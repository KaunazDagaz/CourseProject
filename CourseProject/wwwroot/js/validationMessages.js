document.addEventListener('DOMContentLoaded', function () {
    function localizeValidationMessages() {
        const inputs = document.querySelectorAll('input, select, textarea');
        inputs.forEach(input => {
            if (input.hasAttribute('required')) {
                input.addEventListener('invalid', function (e) {
                    if (e.target.validity.valueMissing) {
                        e.target.setCustomValidity(i18next.t('Validation.Required', 'This field is required'));
                    } else {
                        e.target.setCustomValidity('');
                    }
                });
                input.addEventListener('input', function (e) {
                    e.target.setCustomValidity('');
                });
            }
            if (input.type === 'number') {
                input.addEventListener('invalid', function (e) {
                    if (e.target.validity.rangeOverflow) {
                        const max = e.target.getAttribute('max');
                        e.target.setCustomValidity(i18next.t('Validation.Max', 'Value must be less than or equal to {{max}}', { max: max }));
                    } else if (e.target.validity.rangeUnderflow) {
                        const min = e.target.getAttribute('min');
                        e.target.setCustomValidity(i18next.t('Validation.Min', 'Value must be greater than or equal to {{min}}', { min: min }));
                    } else {
                        e.target.setCustomValidity('');
                    }
                });
            }
            if (input.type === 'text' || input.tagName.toLowerCase() === 'textarea') {
                if (input.hasAttribute('maxlength')) {
                    input.addEventListener('invalid', function (e) {
                        if (e.target.validity.tooLong) {
                            const maxLength = e.target.getAttribute('maxlength');
                            e.target.setCustomValidity(i18next.t('Validation.Maxlength', 'Text must be {{maxlength}} characters or fewer', { maxlength: maxLength }));
                        } else {
                            e.target.setCustomValidity('');
                        }
                    });
                }
            }
        });
    }

    localizeValidationMessages();

    if (typeof i18next !== 'undefined') {
        i18next.on('languageChanged', localizeValidationMessages);
    }

    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            if (mutation.addedNodes && mutation.addedNodes.length > 0) {
                mutation.addedNodes.forEach(function (node) {
                    if (node.nodeType === 1 && (node.tagName === 'INPUT' || node.tagName === 'SELECT' || node.tagName === 'TEXTAREA' ||
                        node.querySelectorAll)) {
                        localizeValidationMessages();
                    }
                });
            }
        });
    });

    observer.observe(document.body, { childList: true, subtree: true });
});