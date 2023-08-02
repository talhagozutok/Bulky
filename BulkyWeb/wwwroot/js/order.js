var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#orderTable').DataTable({
        ajax: {
            url: '/admin/order/getall',
            type: 'GET'
        },
        columns: [
            { data: 'id', width: '5%' },
            { data: 'name', width: '15%' },
            { data: 'phoneNumber', width: '20%' },
            { data: 'applicationUser.email', width: '15%' },
            { data: 'orderStatus', width: '10%' },
            {
                data: 'orderTotal',
                width: '10%',
                render: DataTable.render.number(null, null, 2, '$')
            },
            {
                data: 'id',

                // https://datatables.net/reference/option/columns.data#:~:text=function%20data(%20row%2C%20type%2C%20set%2C%20meta%20)
                // https://stackoverflow.com/a/44209012/22294307
                render: function (data, type, row, meta) {
                    return `<div class="w-75" role="group">
						        <a href="/admin/order/details?=orderId=${data}" class="btn btn-outline-primary">
							        <i class="bi bi-arrow-right"></i> Details
						        </a>
					        </div>`
                },
                title: 'Operations',
                orderable: false,
                width: '25%'
            },
        ],
        language: {
            // url: '//cdn.datatables.net/plug-ins/1.13.5/i18n/tr.json',
            emptyTable: "There aren't any order in the database."
        }
    });
}