$(document).ready(function () {


    $("#EmailNotAvailable").change(function () {
        debugger
        if ($(this).is(":checked")) {
            $("#Email").attr("required", true);
            $("#emailStar").show();   // ⭐ show *
        } else {
            $("#Email").removeAttr("required");
            $("#emailStar").hide();   // ❌ hide *
        }

    });

    //Empty city clears related fields
    $("#CityCode").on("input", function () {

        if ($(this).val().trim() === "") {

            $("#CountryCode").val('');
            $("#StateCode").val('');
            $("#PostCode").empty().append('<option value="">--Select--</option>');

        }

    });

    // CITY AUTOCOMPLETE
    $("#CityCode").autocomplete({

        source: function (request, response) {

            $.get(baseURL + "Vendor/GetCityList",
                { city: request.term },
                function (data) {

                    response($.map(data, function (item) {
                        return {
                            label: item.name,
                            value: item.name
                        };
                    }));

                });

        },

        minLength: 1,

        select: function (event, ui) {



            // 🔥 CLEAR OLD DATA
            $("#CountryCode").val('');
            $("#StateCode").val('');
            $("#PostCode").empty().append('<option value="">--Select--</option>');

            // CITY DETAIL
            $.get(baseURL + "Vendor/GetCityDetail",
                { city: ui.item.value },
                function (data) {
                    
                    if (data) {
                        $("#CityCode").val(data.city);
                        $("#CountryCode").val(data.countryCode || '').trigger('change');;
                        $("#StateCode").val(data.stateCode || '');
                    }

                });

            // POSTCODE LIST
            $.get(baseURL + "Vendor/GetPostCodeList",
                { city: ui.item.value },
                function (data) {

                    $("#PostCode").empty().append('<option value="">--Select--</option>');

                    $.each(data, function (i, item) {

                        $("#PostCode").append(
                            '<option value="' + item.code + '">' + item.name + '</option>'
                        );

                    });

                });

            return false;
        }

    });


    $('#CountryCode').on('change', function () {

        handleVendorCurrency();
        handleVendorPostingGroup();
        handleVendorGenBusPostingGroup();
    });

    $("#GSTVendorType").change(function () {

        var selectedText = $("#GSTVendorType option:selected").text();

        if ($("#GSTVendorType")[0].selectedIndex >= 0) {

            if (selectedText === "Unregistred") {

                $("#AggregateTurnover").prop("selectedIndex", 2);
                $("#GSTReturnFrequency").prop("selectedIndex", -1);

            } else {

                $("#AggregateTurnover").prop("selectedIndex", 1);

            }
        }

    });

    $('#btnSaveVendorMaster').click(function () {
        
        var form = $('#vendorForm');

        if (!form.valid()) return;

        var formData = new FormData(form[0]);

        // disabled fields
        formData.append("AggregateTurnover", $('#AggregateTurnover').val());
        formData.append("StateCode", $('#StateCode').val());
        formData.append("DisplayNo", $('#DisplayNo').text());
        formData.append("MasterCode", $('#MasterCode').val());
        formData.append("CurrencyCode", $('#CurrencyCode').val());
        formData.append("VendorPostingGroup", $('#VendorPostingGroup').val());
        formData.append("GenBusPostingGroup", $('#GenBusPostingGroup').val());

        // 👉 Email + Checkbox validation
        var vendorEmail = $('#Email').val().trim();
        var isCheckedEmailNoAvailable = $('#EmailNotAvailable').is(':checked');

        // ❌ Required when checkbox checked
        if (isCheckedEmailNoAvailable && vendorEmail === "") {
            $('[data-valmsg-for="Email"]').text("Email is required");
            $('#Email').focus();
            return;
        }

        $.ajax({
            url: baseURL + 'Vendor/SaveVendorMaster',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {

                    //alert(response.message);
                    showToast("Vendor Inserted Successfully.", "success", 4000);

                    $('#vendorForm')[0].reset();

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
function handleVendorCurrency() {
    
    var country = $('#CountryCode').val();
    var currency = $('#CurrencyCode');

    if (!country) {
        // ❌ No country selected → reset
        currency.val("");
        currency.prop("disabled", false);
        return;
    }

    if (country === "IN") {
        currency.val("");
        currency.prop("disabled", true);
    } else {
        currency.prop("disabled", false);
    }
}
function handleVendorPostingGroup() {

    var country = $('#CountryCode').val();
    var postingGroup = $('#VendorPostingGroup');

    if (!country) {
        // ❌ Reset
        postingGroup.val("");
        postingGroup.prop("disabled", false);
        return;
    }

    if (country === "IN") {
        postingGroup.val("DOMESTIC");
        postingGroup.prop("disabled", true);
    } else {
        postingGroup.val("FOREIGN");
        postingGroup.prop("disabled", true);
    }
}

function handleVendorGenBusPostingGroup() {

    var country = $('#CountryCode').val();
    var genBusPostingGroup = $('#GenBusPostingGroup');

    if (!country) {
        // ❌ Reset
        genBusPostingGroup.val("");
        genBusPostingGroup.prop("disabled", false);
        return;
    }

    if (country === "IN") {
        genBusPostingGroup.val("DOMESTIC");
        genBusPostingGroup.prop("disabled", true);
    } else {
        genBusPostingGroup.val("EXPORT");
        genBusPostingGroup.prop("disabled", true);
    }
}