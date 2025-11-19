
$(document).ready(function () {
    $('#employeeTable').DataTable({
        "ajax": {
            "url": "/Employee/GetEmployees",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "email" },
            { "data": "country" },
            { "data": "state" },
            { "data": "city" },
            { "data": "panNo" },
            { "data": "passportNo" },
            { "data": "gender" },
            {
                "data": "isActive",
                "render": function (data) {
                    return data ? 'Yes' : 'No';
                }
            },
            {
                "data": "profileImage",
                "render": function (data) {
                    return data
                        ? `<img src="/uploads/${data}" width="50" height="50" class="rounded-circle" />`
                        : "No Image";
                },
                "orderable": false
            },
            {
                "data": "rowId",
                "render": function (data) {
                    return `
                        <a href="/Employee/CreateOrEdit/${data}" class="btn btn-sm btn-primary">Edit</a>
                        <button class="btn btn-sm btn-danger deleteEmployee" data-id="${data}">Delete</button>`;
                },
                "orderable": false
            }
        ]
    });

    // Delete functionality
    $('#employeeTable').on('click', '.deleteEmployee', function () {
        var employeeId = $(this).data('id');
        Swal.fire({
            title: 'Are you sure?',
            text: "You want to delete this employee!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Employee/DeleteEmployee',
                    type: 'POST',
                    data: { id: employeeId },
                    success: function (res) {
                        if (res.success) {
                            Swal.fire('Deleted!', 'Employee has been deleted.', 'success');
                            $('#employeeTable').DataTable().ajax.reload();
                        } else {
                            Swal.fire('Error!', res.message, 'error');
                        }
                    }
                });
            }
        });
    });
});
