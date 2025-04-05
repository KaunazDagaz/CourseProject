function showMessage(message, isError = true) {
    const $toast = $('#messageToast');
    const $messageBox = $('#messageBox');
    $toast.removeClass('bg-success bg-danger text-white');
    if (isError) {
        $toast.addClass('bg-danger text-white');
    } else {
        $toast.addClass('bg-success text-white');
    }
    $messageBox.text(message);
    const toastInstance = new bootstrap.Toast($toast[0], {
        delay: 5000,
        autohide: true
    });
    toastInstance.show();
}

window.showMessage = showMessage;