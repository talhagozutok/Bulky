var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#companyTable').DataTable({
        ajax: {
            url: '/admin/company/getall',
            type: 'GET'
        },
        columnDefs: [{
            targets: '_all',
            render: function (data, type, row) {
                return type === 'display' && data.length > 20 ?
                    data.substr(0, 20) + '...' :
                    data;
            }
        }],
        columns: [
            { data: 'name', width: '15%' },
            { data: 'city', width: '15%' },
            { data: 'state', width: '15%' },
            { data: 'streetAddress', width: '15%' },
            { data: 'phoneNumber', width: '15%' },
            {
                data: 'id',

                // https://datatables.net/reference/option/columns.data#:~:text=function%20data(%20row%2C%20type%2C%20set%2C%20meta%20)
                // https://stackoverflow.com/a/44209012/22294307
                "render": function (data, type, row, meta) {
                    return `<div class="btn-group" role="group">
						        <a href="/admin/company/upsert/${data}"
                                    class="btn btn-outline-primary">
							        <i class="bi bi-pencil-square"></i> Edit
						        </a>
						        <a href="/admin/company/delete/${data}"
                                    class="btn btn-outline-danger"
                                    onclick="deleteCompany(event, '${row['name']}')">
							        <i class="bi bi-trash-fill"></i> Delete
						        </a>
					        </div>`
                },
                title: 'Operations',
                orderable: false,
                width: '25%'
            },
        ],
        language: {
            //url: '//cdn.datatables.net/plug-ins/1.13.5/i18n/tr.json',
            emptyTable: "There aren't any company in the database."
        }
    });
}

function deleteCompany(e, companyName) {
    // preventing default event which is redirect for anchor tag.
    e.preventDefault();

    // do my commands
    DeleteAlert.fire({
        title: 'Delete this company?',
        html: `'<strong>${companyName}</strong>' will be deleted.`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: e.target.href,
                type: 'DELETE',
                success: function () {
                    dataTable.ajax.reload();
                    fireToastDelete('Company deleted successfully.', `Company name: ${companyName}`)
                }
            })
        }
    })
}