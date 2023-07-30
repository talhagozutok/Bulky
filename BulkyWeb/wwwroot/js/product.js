var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#productTable').DataTable({
        ajax: {
            url: '/admin/product/getall',
            type: 'GET'
        },
        columns: [
            { data: 'title', width: '25%' },
            { data: 'isbn', width: '15%' },
            { data: 'listPrice', width: '10%' },
            { data: 'author', width: '15%' },
            { data: 'category.name', width: '10%' },
            {
                data: 'id',

                // https://datatables.net/reference/option/columns.data#:~:text=function%20data(%20row%2C%20type%2C%20set%2C%20meta%20)
                // https://stackoverflow.com/a/44209012/22294307
                render: function (data, type, row, meta) {
                    return `<div class="w-75" role="group">
						        <a href="/admin/product/upsert/${data}" class="btn btn-outline-primary">
							        <i class="bi bi-pencil-square"></i> Edit
						        </a>
						        <a href="/admin/product/delete/${data}" class="btn btn-outline-danger" onclick="deleteProduct(event, '${row['title']}')">
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
            // url: '//cdn.datatables.net/plug-ins/1.13.5/i18n/tr.json',
            emptyTable: "There aren't any product in the database."
        }
    });
}

function deleteProduct(e, productName) {
    // preventing default event which is redirect for anchor tag.
    e.preventDefault();

    // do my commands
    DeleteAlert.fire({
        title: 'Are you sure?',
        html: `The product '<strong>${productName}</strong>' will be deleted.`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: e.target.href,
                type: 'DELETE',
                success: function () {
                    dataTable.ajax.reload();
                    fireToastDelete('Product deleted successfully.', `Product title: ${productName}`)
                }
            })
        }
    })
}