var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = new DataTable('#tblData', {
        ajax: '/admin/order/getall',
        columns: [
            { data: 'id', width: '15%' },
            { data: 'name', width: '15%' },
            { data: 'phoneNumber', width: '15%' },
            { data: 'applicationUser.email', width: '15%' },
            { data: 'orderStatus', width: '15%' },
            { data: 'orderTotal', width: '15%' },
            {
                data: 'id',
                width: '25%',
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/order/detail?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil square"></i></a>
                    </div>`;
                }
            }
        ]
    });
}
