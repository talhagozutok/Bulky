var dataTable;

$(document).ready(function () {
    var url = window.location.search;

    if (url.includes("inProcess")) {
        loadDataTable("inProcess");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("paymentPending")) {
        loadDataTable("paymentPending");
    }
    else if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        loadDataTable("all");
    }
});

function loadDataTable(status) {
    dataTable = $('#orderTable').DataTable({
        ajax: {
            url: `/admin/order/getall?status=${status}`,
            type: 'GET'
        },
        columns: [
            { data: 'id', width: '5%' },
            { data: 'name', width: '25%' },
            { data: 'phoneNumber', width: '20%' },
            { data: 'applicationUser.email', width: '20%' },
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
						        <a href="/admin/order/details?orderId=${data}" class="btn btn-outline-primary">
							        <i class="bi bi-arrow-right"></i>
						        </a>
					        </div>`
                },
                title: 'Details',
                orderable: false,
                width: '10%'
            },
        ],
        language: {
            // url: '//cdn.datatables.net/plug-ins/1.13.5/i18n/tr.json',
        }
    });
}