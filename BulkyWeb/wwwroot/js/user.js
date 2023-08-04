var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#userTable').DataTable({
        ajax: {
            url: '/admin/user/getall',
            type: 'GET'
        },
        columns: [
            { data: 'name', width: '15%' },
            { data: 'email', width: '15%' },
            { data: 'phoneNumber', width: '15%' },
            { data: 'company.name', width: '15%' },
            { data: '', width: '15%' },
            {
                data: 'id',
                render: function (data) {
                    return `<div class="btn-group" role="group">
						        <a href="/admin/user/upsert/${data}" class="btn btn-outline-primary">
							        <i class="bi bi-pencil-square"></i> Edit
						        </a>
					        </div>`
                },
                width: '25%',
                orderable: false,
                title: 'Operations'
            }
        ]
    });
}