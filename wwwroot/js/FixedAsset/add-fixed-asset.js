$(document).ready(function () {

    $(".fixed-asset-datepicker").datepicker({
        dateFormat: "dd/mm/y",   // 👈 dd/MM/yyyy format
        changeMonth: true,
        changeYear: true
    });

    $('#DepreciationMethod').change(function () {

        var selectedText = $('#DepreciationMethod option:selected').text().trim();

        if (selectedText === 'Straight-Line') {

            $('#StraightLinePercent').prop('disabled', false);
            $('#DecliningBalancePercent').prop('disabled', true).val('0');

        }
        else if (selectedText === 'Declining-Balance 1') {

            $('#DecliningBalancePercent').prop('disabled', false);
            $('#StraightLinePercent').prop('disabled', true).val('0');

        }
        else {

            $('#StraightLinePercent').prop('disabled', false);
            $('#DecliningBalancePercent').prop('disabled', false);

        }

    });

    $('#Division').change(function () {

        $('#MainAssetComponent').val('');

        $('#ComponentOfMainAsset').empty()
            .append('<option value="">--Select--</option>');
    });


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
        formData.append("Division", $("#Division").val());
        formData.append("DivisionStr", $('#Division option:selected').text());
        formData.append("StraightLinePercent", $("#StraightLinePercent").val());
        formData.append("DecliningBalancePercent", $("#DecliningBalancePercent").val());

        showLoader();

        $.ajax({
            url: baseURL + 'FixedAsset/SaveFixedAssetMaster',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {
                    hideLoader();
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

                        hideLoader();

                    } else {

                        hideLoader();
                        alert(response.message);
                        
                    }

                }

            }
        });
    });

    $('#MainAssetComponent').change(function () {
        
        var division = $('#Division').val();
        var divisionText = $('#Division option:selected').text();

        // Check Division selected or not
        if (!division || division === '') {
            showToast('Please select Division first','danger');

            // reset dropdown
            $('#ComponentOfMainAsset').empty();
            $('#ComponentOfMainAsset').append('<option value="">--Select--</option>');
            return;
        }

        var selectedText = $(this).find("option:selected").text();

        if (selectedText && selectedText.trim() === 'Main Asset') {

            $('#ComponentOfMainAsset').empty()
                .append('<option value="">--Select--</option>');

            return;
        }


        $('#ComponentOfMainAsset').empty();
        $('#ComponentOfMainAsset').append('<option value="">--Select--</option>');

        $.ajax({
            url: baseURL + 'FixedAsset/GetFixedAssetComponetWithDivision',
            type: 'GET',
            data: {
                division: divisionText
            },
            success: function (data) {

                $.each(data, function (i, item) {
                    $('#ComponentOfMainAsset').append(
                        $('<option>', {
                            value: item.code,
                            text: item.name
                        })
                    );
                });

            },
            error: function () {
                alert('Error loading Component data');
            }
        });

    });

    $('#DepreciationMethod').val('0').trigger("change")
    $("#DepreciationBookCode").val("COMPANY")
});