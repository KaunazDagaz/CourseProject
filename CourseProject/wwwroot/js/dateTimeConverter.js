document.addEventListener('DOMContentLoaded', function () {
    convertTimestampsToLocalTime();
});

function convertTimestampsToLocalTime() {
    const timestampElements = document.querySelectorAll('[data-utc-time]');
    timestampElements.forEach(element => {
        const utcTime = element.getAttribute('data-utc-time');
        const date = new Date(utcTime);
        const options = {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        };
        const localTime = date.toLocaleString(undefined, options);
        element.textContent = localTime;
    });
}

window.convertTimestampsToLocalTime = convertTimestampsToLocalTime;