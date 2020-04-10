$(window).on('load', function () {
    if ($('#myModal').length) {
        $('#myModal').modal('show');

        $("#RegistrationInfo_IdentitcationNumber").val("");
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

    $('#initiateYear').datetimepicker({
        locale: 'vi',
        format: 'YYYY',
        date: initialDate,
        ignoreReadonly: true,
        allowInputToggle: true
    });

    $('#initiateMonth').datetimepicker({
        locale: 'vi',
        format: 'MMMM',
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
        locale: 'vi',
        format: 'LT',
        stepping: 30,
        date: startTime,
        ignoreReadonly: true,
        allowInputToggle: true
    });

    $('#endTime').datetimepicker({
        locale: 'vi',
        format: 'LT',
        stepping: 30,
        date: endTime,
        ignoreReadonly: true,
        allowInputToggle: true
    });
});


$('#btnSubmit').click(function () {
    initialDate = new Date(
        $("#initiateYear").data("DateTimePicker").date()._d.getFullYear(),
        $("#initiateMonth").data("DateTimePicker").date()._d.getMonth()
    );
    $("#RegistrationInfo_InitiateDate").val(initialDate.toUTCString());

    startTime = new Date(
        $("#meditaionDate").data("DateTimePicker").date()._d.getFullYear(),
        $("#meditaionDate").data("DateTimePicker").date()._d.getMonth(),
        $("#meditaionDate").data("DateTimePicker").date()._d.getDate(),
        $("#startTime").data("DateTimePicker").date()._d.getHours(),
        $("#startTime").data("DateTimePicker").date()._d.getMinutes()
    );
    $("#RegistrationInfo_StartTime").val(startTime.toUTCString());

    endTime = new Date(
        $("#meditaionDate").data("DateTimePicker").date()._d.getFullYear(),
        $("#meditaionDate").data("DateTimePicker").date()._d.getMonth(),
        $("#meditaionDate").data("DateTimePicker").date()._d.getDate(),
        $("#endTime").data("DateTimePicker").date()._d.getHours(),
        $("#endTime").data("DateTimePicker").date()._d.getMinutes()
    );
    $("#RegistrationInfo_EndTime").val(endTime.toUTCString());
});