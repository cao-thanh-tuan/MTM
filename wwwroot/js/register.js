$(window).on('load', function () {
    if ($('#myModal').length) {
        $('#myModal').modal('show');

        $("#RegistrationInfo_Phone").val("");
    }

    var initialDate, startTime, endTime;

    if ($(".field-validation-error").length !== 0 || $(".validation-summary-errors").length !== 0) {
        initialDate = new Date($("#RegistrationInfo_InitiateDate").val());
        startTime = new Date($("#RegistrationInfo_StartTime").val());
        endTime = new Date($("#RegistrationInfo_EndTime").val());
    } else {
        initialDate = new Date();
        startTime = new Date();
        endTime = new Date();

        startTime.setHours(startTime.getHours() + 1);
        startTime.setMinutes(0);
        endTime.setHours(endTime.getHours() + 3);
    }

    $('#initiateDate').datetimepicker({
        locale: 'vi',
        format: 'L',
        date: initialDate,
        ignoreReadonly: true,
        allowInputToggle: true
    });

    $('#meditaionDate').datetimepicker({
        locale: 'vi',
        format: 'L',
        date: startTime,
        minDate: startTime,
        ignoreReadonly: true,
        allowInputToggle: true
    });

    $('#startTime').datetimepicker({
        viewMode: 'times',
        locale: 'vi',
        format: 'LT',
        stepping: 30,
        date: startTime,
        ignoreReadonly: true,
        allowInputToggle: true
    });

    $('#endTime').datetimepicker({
        viewMode: 'times',
        locale: 'vi',
        format: 'LT',
        stepping: 30,
        date: endTime,
        ignoreReadonly: true,
        allowInputToggle: true
    });
});



$(function () {
    $("#btnSubmit").click(function () {
        initialDate = new Date(
            $("#initiateDate").datetimepicker("viewDate")._d.getFullYear(),
            $("#initiateDate").datetimepicker("viewDate")._d.getMonth(),
            $("#initiateDate").datetimepicker("viewDate")._d.getDate()
        );
        $("#RegistrationInfo_InitiateDate").val(initialDate.toUTCString());

        startTime = new Date(
            $("#meditaionDate").datetimepicker("viewDate")._d.getFullYear(),
            $("#meditaionDate").datetimepicker("viewDate")._d.getMonth(),
            $("#meditaionDate").datetimepicker("viewDate")._d.getDate(),
            $("#startTime").datetimepicker("viewDate")._d.getHours(),
            $("#startTime").datetimepicker("viewDate")._d.getMinutes()
        );
        $("#RegistrationInfo_StartTime").val(startTime.toUTCString());

        endTime = new Date(
            $("#meditaionDate").datetimepicker("viewDate")._d.getFullYear(),
            $("#meditaionDate").datetimepicker("viewDate")._d.getMonth(),
            $("#meditaionDate").datetimepicker("viewDate")._d.getDate(),
            $("#endTime").datetimepicker("viewDate")._d.getHours(),
            $("#endTime").datetimepicker("viewDate")._d.getMinutes()
        );
        $("#RegistrationInfo_EndTime").val(endTime.toUTCString());

        this.form.submit()
    })
}); 
