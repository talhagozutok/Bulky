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
            { data: 'roles', width: '15%' },
            {
                data: { id: 'id', lockoutEnd: 'lockoutEnd' },
                render: function (data, type, row, meta) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (!row['roles'].includes('Admin')) {
                        // If user has not admin role
                        // show lock/unlock operations

                        if (lockout > today) {
                            // User is currently locked
                            // we need to display button to unlock their account.

                            return `<div class="btn-group" role="group">
						        <a onclick="LockUnlock('${data.id}')" class="btn btn-outline-success">
							        <i class="bi bi-unlock-fill"></i> Unlock
						        </a>
					        </div>`
                        }
                        else {
                            // User is currently unlocked

                            return `<div class="btn-group" role="group">
						        <a onclick="LockUnlock('${data.id}')" class="btn btn-outline-danger">
							        <i class="bi bi-lock-fill"></i> Lock
						        </a>
					        </div>`
                        }
                    }
                    else {
                        // If user has admin role
                        return '';
                    }
                },
                width: '10%',
                orderable: false,
                title: 'Operations'
            }
        ]
    });
}

function LockUnlock(id) {
    $.ajax({
        url: '/admin/user/lockunlock',
        type: 'POST',
        data: JSON.stringify(id),
        contentType: 'application/json',
        success: function (data) {
            if (data.success) {
                fireToast('success', data.message);
                dataTable.ajax.reload();
            }
        }
    })
} 