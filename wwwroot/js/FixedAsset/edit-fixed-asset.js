$(function () {
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

    $('#MainAssetComponent').change(function () {
        
        var division = $('#Division').val();
        var divisionText = $('#Division option:selected').text();

        // Check Division selected or not
        if (!division || division === '') {
            showToast('Please select Division first', 'danger');

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

    $('#btnUpdateFixedAssetMaster').click(function () {

        var form = $('#editFixedAssetForm');

        if (!form.valid()) return;

        var formData = new FormData(form[0]);

        // disabled fields
        formData.append("DisplayNo", $('#DisplayNo').text());
        formData.append("CompanyCode", $('#CompanyCode').val());
        formData.append("Division", $("#Division").val());
        formData.append("DivisionStr", $('#Division option:selected').text());
        formData.append("StraightLinePercent", $("#StraightLinePercent").val());
        formData.append("DecliningBalancePercent", $("#DecliningBalancePercent").val());

        $.ajax({
            url: baseURL + 'FixedAsset/UpdateFixedAssetMaster',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {

                    //alert(response.message);
                    showToast("Fixed Asset Updated Successfully.", "success", 4000);

                    $('#listBtn').click();

                    //$('.text-danger').text('');

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

    editFixedAssetBlockDisableForm();
})

function backFixedAssetListPage() {
    editFixedAssetNo = '';
    $('#listBtn').click();
}

function toggleFixedAssetUnBlock(btn) {

    var fixedAssetCode = btn.getAttribute("data-fixed-asset");

    if (!confirm("Are you sure you want to unblock this fixed asset?")) {
        return; // ❌ cancel
    }

    $.ajax({
        url: baseURL + 'FixedAsset/FixedAssetUnblock',
        type: 'POST',
        data: {
            fixedAssetCode: fixedAssetCode,
            displayNo: $('#DisplayNo').text(),
            fixedAssetDescription: $('#Description').val()
        },
        success: function (res) {

            if (res.success) {

                showToast(res.message, "success", 3000);

                btn.style.display = "none";

            } else {
                alert(res.message || "Something went wrong!");
            }
        }
    });
}
function editFixedAssetBlockDisableForm() {
    var blockedVal = $("#Blocked").val();

    if (blockedVal == "0") {
        // disable all inputs inside form
        $("#editFixedAssetForm")
            .find("input, select, textarea, button")
            .prop("disabled", true);

        $("#btnBackFixedAssetListPage").prop("disabled", false);
        $("#btnUpdateFixedAssetMaster").hide();
    } else {
        return;
    }
}