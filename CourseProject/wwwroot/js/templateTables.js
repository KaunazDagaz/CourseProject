(function () {
    $(document).ready(function () {
        if ($.fn.DataTable && $('#createdTemplatesTable').length) {
            const createdTable = $('#createdTemplatesTable').DataTable({
                "paging": false,
                "info": false,
                "searching": false,
                "autoWidth": false,
                "bJQueryUI": false,
                "bAutoWidth": false,
                "ordering": true,
                "dom": 't',
                "columnDefs": [
                    { "orderable": true, "targets": [0, 1, 2] }
                ],
                "order": [[2, 'desc']]
            });
        }

        if ($.fn.DataTable && $('#filledTemplatesTable').length) {
            const filledTable = $('#filledTemplatesTable').DataTable({
                "paging": false,
                "info": false,
                "searching": false,
                "autoWidth": false,
                "bJQueryUI": false,
                "bAutoWidth": false,
                "ordering": true,
                "dom": 't',
                "columnDefs": [
                    { "orderable": true, "targets": [0, 1, 2] }
                ],
                "order": [[2, 'desc']]
            });
        }

        $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
            $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
        });
    });
})();