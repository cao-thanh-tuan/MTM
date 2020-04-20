$(function () {

    $("button[id^='btnDelete_']").on('click', function (evt) {
        evt.preventDefault();
        var id = $(this).prop("value");
        var name = $(this).prop("name");
        var info = { "ID": id };

        $.ajax({
            type: "GET",
            url: "./classes/index?handler=CanDelete&id=" + id,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            success: function (result) {
                if (!result) {
                    Swal.fire({
                        title: '<h5>Không thể xóa lớp có danh sách đồng tu!</h5>',
                        text: 'Vui lòng xóa hoặc chuyển danh sách đồng tu sang lớp khác trước khi xóa lớp',
                        width: 350,
                        type: 'error',
                    });
                } else {
                    Swal.fire({
                        type: 'warning',
                        title: '<h5>Bạn có chắc muốn xóa ' + name + '?</h5>',
                        width: 350,
                        showCancelButton: true,
                        cancelButtonText: 'Hủy',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Đồng Ý',
                        allowOutsideClick: () => !Swal.isLoading(),
                    }).then((result) => {
                        if (result.value) {
                            $.ajax({
                                type: "POST",
                                url: "./classes/index?handler=Delete",
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify(info),
                                success: function (result) {
                                    if (result) {
                                        Swal.fire({
                                            title: '<h5>Xóa thông tin lớp thành công</h5>',
                                            width: 350,
                                            type: 'success',
                                        }).then((result) => {
                                            location.reload(true);
                                        });
                                    } else {
                                        Swal.fire({
                                            title: '<h5>Xóa thông tin lớp không thành công!</h5>',
                                            width: 350,
                                            type: 'error',
                                        })
                                    }
                                }
                            });
                        }
                    });
                }
            }
        });
    });
}); 
