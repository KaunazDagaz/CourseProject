i18next.on('initialized', function () {
    $(function () {
        initializeCheckboxes();

        function initializeCheckboxes() {
            $("#selectAll").on('change', function () {
                $(".user-checkbox").prop('checked', $(this).prop('checked'));
            });
            $(document).on('change', '.user-checkbox', function () {
                $("#selectAll").prop('checked', $('.user-checkbox:checked').length === $('.user-checkbox').length);
            });
        }

        function getSelectedUserIds() {
            const userIds = [];
            $('.user-checkbox:checked').each(function () {
                userIds.push($(this).val());
            });
            return userIds;
        }

        function handleUserAction(action, userIds) {
            if (userIds.length === 0) {
                showMessage(i18next.t('selectAtLeastOne'));
                return;
            }
            const buttons = $("#blockBtn, #unblockBtn, #deleteBtn, #addAdminBtn, #removeAdminBtn");
            buttons.prop('disabled', true);
            $.ajax({
                url: `/Admin/${action}`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(userIds),
                headers: {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
                },
                success: function (response) {
                    if (response.success) {
                        updateTable();
                        showMessage(i18next.t('operationSuccess'), false);
                    } else if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    }
                },
                error: function () {
                    buttons.prop('disabled', false);
                    showMessage(i18next.t('errorOccurred'));
                }
            });
        }

        function updateTable() {
            $.get('/Admin/GetUsersTable', function (data) {
                $('#tableContainer').html(data);
                localizePage();
                initializeCheckboxes();
                const buttons = $("#blockBtn, #unblockBtn, #deleteBtn, #addAdminBtn, #removeAdminBtn");
                buttons.prop('disabled', false);
            });
        }

        $("#blockBtn").on('click', function () {
            handleUserAction("Block", getSelectedUserIds());
        });

        $("#unblockBtn").on('click', function () {
            handleUserAction("Unblock", getSelectedUserIds());
        });

        $("#deleteBtn").on('click', function () {
            handleUserAction("Delete", getSelectedUserIds());
        });

        $("#addAdminBtn").on('click', function () {
            handleUserAction("AddAdmin", getSelectedUserIds());
        });

        $("#removeAdminBtn").on('click', function () {
            handleUserAction("RemoveAdmin", getSelectedUserIds());
        });
    });
});