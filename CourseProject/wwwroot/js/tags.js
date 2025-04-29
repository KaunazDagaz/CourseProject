document.addEventListener('DOMContentLoaded', function () {
    const tagInput = document.querySelector('#tagInput');
    const tagsHidden = document.querySelector('#tagsHidden');
    const tagify = new Tagify(tagInput, {
        dropdown: {
            maxItems: 5,
            enabled: 0,
            closeOnSelect: false
        },
        enforceWhitelist: false,
        maxTags: 10
    });

    const form = document.getElementById('templateForm');
    if (form) {
        form.addEventListener('submit', function () {
            const tags = tagify.value;
            tagsHidden.value = tags.map(t => t.value).join(',');
        });
    }

    let controller;
    tagify.on('input', function (e) {
        const value = e.detail.value;
        controller && controller.abort();
        controller = new AbortController();
        tagify.loading(true);
        fetch(`/Tag/Search?tagsModel=${encodeURIComponent(value)}`, {
            signal: controller.signal
        })
            .then(res => {
                return res.json();
            })
            .then(data => {
                tagify.whitelist = data.map(item => item.name);
                tagify.loading(false);
                tagify.dropdown.show(value);
            })
            .catch(_ => {
                tagify.loading(false);
            });
    });
});