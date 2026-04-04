$(document).ready(function () {

    $('#GSTGroupCode').change(function () {

        var gstCode = $(this).val();

        $('#HSNSACCode').empty();
        $('#HSNSACCode').append('<option value="">--Select--</option>');

        if (gstCode !== '') {

            $.ajax({
                url: baseURL + 'FixedAsset/GetFixedAssetHSNDataWithGSTGroupCode',
                type: 'GET',
                data: { gstGroupCode: gstCode },
                success: function (data) {

                    $.each(data, function (i, item) {
                        $('#HSNSACCode').append(
                            $('<option>', {
                                value: item.code,
                                text: item.name
                            })
                        );
                    });

                },
                error: function () {
                    alert('Error loading HSN/SAC data');
                }
            });

        }
    });

    $('#btnSaveFixedAssetMaster').click(function () {

        var form = $('#fixedAssetForm');

        if (!form.valid()) return;

        var formData = new FormData(form[0]);

        // disabled fields
        formData.append("DisplayNo", $('#DisplayNo').text());
        formData.append("MasterCode", $('#MasterCode').val());

        $.ajax({
            url: baseURL + 'FixedAsset/SaveFixedAssetMaster',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {

                    //alert(response.message);
                    showToast("Fixed Asset Inserted Successfully.", "success", 4000);

                    $('#fixedAssetForm')[0].reset();

                    $('.text-danger').text('');

                }
                else {

                    $('.text-danger').text('');

                    if (response.errors) {

                        $.each(response.errors, function (key, messages) {

                            var errorSpan = $('[data-valmsg-for="' + key + '"]');

                            if (errorSpan.length) {

                                errorSpan.text(messages[0]);

                            }

                        });

                    } else {

                        alert(response.message);

                    }

                }

            }
        });
    });

});