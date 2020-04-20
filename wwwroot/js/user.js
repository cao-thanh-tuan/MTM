$(function () {

    $("button[id^='btnChangePassword_']").on('click', function (evt) {
        evt.preventDefault();
        var username = $(this).prop("value");

        Swal.fire({
            title: '<h5>Vui lòng nhập mật khẩu mới cho người dùng ' + username + '</h5>',
            input: 'password',
            inputPlaceholder: 'Mật khẩu mới',
            inputAttributes: {
                minlength: 4,
                maxlength: 15,
                autocapitalize: 'off',
                autocorrect: 'off'
            },
            width: 350,
            showCancelButton: true,
            cancelButtonText: 'Hủy',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Đồng Ý',
            allowOutsideClick: () => !Swal.isLoading(),
            inputValidator: (value) => {
                return new Promise((resolve) => {
                    if (/^[a-zA-Z0-9]{4,15}$/.test(value)) {
                        resolve()
                    } else {
                        resolve('Mật Khẩu chỉ chứa số hoặc ký tự và dài từ 4 đến 15 ký tự')
                    }
                })
            }
        }).then((result) => {
            if (result.value) {
                var user = { "Username": username, "Password": result.value };

                $.ajax({
                    type: "POST",
                    url: "./users/index?handler=ChangePassword",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(user),
                    success: function (result) {
                        if (result) {
                            Swal.fire({
                                title: '<h5>Cập nhật mật khẩu thành công</h5>',
                                width: 350,
                                type: 'success',
                            })
                        } else {
                            Swal.fire({
                                title: '<h5>Cập nhật mật khẩu không thành công!</h5>',
                                width: 350,
                                type: 'error',
                            })
                        }
                    }
                });
            }
        });
    });

    $("button[id^='btnDelete_']").on('click', function (evt) {
        evt.preventDefault();
        var username = $(this).prop("value");

        Swal.fire({
            type: 'warning',
            title: '<h5>Bạn có chắc muốn xóa người dùng ' + username + '?</h5>',
            width: 350,
            showCancelButton: true,
            cancelButtonText: 'Hủy',
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Đồng Ý',
            allowOutsideClick: () => !Swal.isLoading(),
        }).then((result) => {
            if (result.value) {
                var user = { "Username": username };

                $.ajax({
                    type: "POST",
                    url: "./users/index?handler=Delete",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(user),
                    success: function (result) {
                        if (result) {
                            Swal.fire({
                                title: '<h5>Xóa thông tin người dùng thành công</h5>',
                                width: 350,
                                type: 'success',
                            }).then((result) => {
                                location.reload(true);
                            });
                        } else {
                            Swal.fire({
                                title: '<h5>Xóa thông tin người dùng không thành công!</h5>',
                                width: 350,
                                type: 'error',
                            })
                        }
                    }
                });
            }
        });
    });
}); 
