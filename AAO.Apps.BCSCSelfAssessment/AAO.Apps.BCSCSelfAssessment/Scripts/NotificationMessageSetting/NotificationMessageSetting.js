var notificationMessageSettingTable;
$(document).ready(function () {
    notificationMessageSettingTable = $('#NotificationMessageSettingTable').dataTable({
        "ajax": {
            "type": "POST",
            "url": '/NotificationMessageSetting/GetAllNotificationMessageSetting',
        },
        "lengthMenu": [[25, 50, 75, 100], [25, 50, 75, 100]],
        "language": {
            "processing": "<img src='/Content/img/Processing.gif'> Loading...",
            "lengthMenu": "Show _MENU_ ",
            "sEmptyTable": "No Records found",
            "info": "Showing _START_ - _END_ of _TOTAL_ entries",
        },
        "paging": true,
        "processing": true,
        "serverSide": true,
        "filter": false,



        "columns": [
            { "data": "NotificationMessage", "bSortable": false, "name": "NotificationMessage", "width": "70%" },
            { "data": "NoOfTime", "bSortable": false, "name": "NoOfTime", "width": "7%" },
            /* { "data": "OnholdScreenTime", "bSortable": false, "name": "OnholdScreenTime", "width": "6%" },*/
            /* { "data": "IsUnable", "bSortable": false, "name": "IsUnable", "width": "8%" },*/
            {
                data: null, "width": "7%", "bSortable": false, "name": "IsUnable", render: function (data, type, row) {
                    return data.IsUnable == true ? 'Yes' : 'No';
                }
            },
            {
                "data": "NotificationMessageSettingId",
                "bSortable": false,
                "name": "IsUnable",
                "render": function (data, type, row, meta) {
                    return '<button type="button" class="btn btn-info" onclick="openMessageSettingForm(' + data + ')" data-toggle="modal">Edit</button>' +
                        '<button type="button" class="btn btn-danger margin-br-10" onclick="deleteMessageSetting(' + data + ')" data-toggle="modal">Delete</button>';
                }
            }
        ],
    });



});



function openMessageSettingForm(id) {

    $.get("/NotificationMessageSetting/GetMessageSettingForm" + "?id=" + id).done(function (responce) {

        $("#MessageSettingBody").html(responce);



        $('#MessageSettingModal').modal('show');
    });
}



function deleteMessageSetting(_id) {



    $.ajax({
        type: 'post',
        async: false,
        data: { id: _id },
        url: '/NotificationMessageSetting/DeleteMessageSetting',
        contentType: "application/x-www-form-urlencoded",
        dataType: "json",
        success: function (res) {
            if (res.isSucess) {
                createAlert('Success', '', res.msg, 'success', true, true, 'pageMessages');
                //createNotification(res.msg, "success");
            } else {
                createAlert('Error', '', res.msg, 'danger', true, true, 'pageMessages');
                // createNotification(res.msg, "error");
            }
            $('#NotificationMessageSettingTable').DataTable().ajax.reload()
        },
        error: function (err) {
            createAlert('Error', '', 'Error while processing message settings.', 'danger', true, true, 'pageMessages');
            //createNotification("Error while processing message settings.", "error");
            console.log(err);
        }
    })
}



saveNotificationMessageSetting = form => {
    var isEnable = $("#IsUnable_Value").is(":checked") ? 1 : 0;
    var notificationMessageSettingId = $("#NotificationMessageSettingId").val();
    var notificationMessage = $("#NotificationMessage").val();
    var noOfTime = $("#NoOfTime").val();
    var onholdScreenTime = 0;
    $.validator.unobtrusive.parse(form);
    if ($(form).valid()) {
        try {
            $.ajax({
                type: 'post',
                async: false,
                data: JSON.stringify({ NotificationMessageSettingId: notificationMessageSettingId, NotificationMessage: notificationMessage, NoOfTime: noOfTime, OnholdScreenTime: onholdScreenTime, IsEnable: isEnable }),
                url: '/NotificationMessageSetting/SaveNotificationMessageSetting',
                // contentType: "application/x-www-form-urlencoded",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.isSucess) {
                        if (notificationMessageSettingId > 0) {
                            createAlert('Success', '', 'Message settings edit successfully.', 'success', true, true, 'pageMessages');
                            //createNotification("Message settings edit successfully.", "success");
                        } else {
                            createAlert('Success', '', 'Message settings save successfully.', 'success', true, true, 'pageMessages');
                            //createNotification("Message settings save successfully.", "success");
                        }

                        $('#MessageSettingModal').modal('hide');
                        $('#NotificationMessageSettingTable').DataTable().ajax.reload()
                    }
                    else {
                        // $('#form-modal .modal-body').html(res.html);
                    }
                },
                error: function (err) {
                    createAlert('Error', '', 'Error while processing message settings.', 'danger', true, true, 'pageMessages');
                    //createNotification("Error while processing message settings.", "error");
                    console.log(err);
                }
            })
            return false;
        } catch (ex) {
            console.log(ex);
        }
    } else {
        return false;
    }
}
function limit(element, max_chars) {
    if (element.value.length > max_chars) {
        element.value = element.value.substr(0, max_chars);
    }
}