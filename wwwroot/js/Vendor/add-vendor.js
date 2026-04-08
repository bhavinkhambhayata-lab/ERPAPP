$(document).ready(function () {

    $('.searchable-dropdown').select2({
        theme: "bootstrap-5",   // 🔥 IMPORTANT
        placeholder: "--Select--",
        allowClear: true,
        width: '100%'
    });

    $('.vendor-datepicker').datepicker({
        dateFormat: "dd/mm/yy", 
        changeMonth: true,
        changeYear: true,

        onSelect: function () {
            var input = $(this);
            var date = input.datepicker('getDate');

            formatDate(input, date);
            clearError(input);
        }
    });

   
    $('.vendor-datepicker').on('blur', function () {

        var input = $(this);
        var value = input.val().trim();

        clearError(input);

        if (value === '') return;

        var regex = /^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/\d{4}$/;

        if (!regex.test(value)) {
            showError(input, "Invalid date format (dd/mm/yyyy)");
            return;
        }

        try {
            var parsedDate = $.datepicker.parseDate('dd/mm/yy', value);

            formatDate(input, parsedDate);
            input.datepicker('setDate', parsedDate);

        } catch (e) {
            showError(input, "Invalid date");
        }
    });

    function formatDate(input, date) {
        var day = ("0" + date.getDate()).slice(-2);
        var month = ("0" + (date.getMonth() + 1)).slice(-2);
        var year = date.getFullYear();

        input.val(day + '/' + month + '/' + year);
    }

    function showError(input, message) {
        var span = $('[data-valmsg-for="' + input.attr('name') + '"]');

        input.addClass('input-validation-error');
        span.text(message);
    }

    function clearError(input) {
        var span = $('[data-valmsg-for="' + input.attr('name') + '"]');

        input.removeClass('input-validation-error');
        span.text('');
    }

    $("#EmailNotAvailable").change(function () {

        if ($(this).is(":checked")) {
            $("#Email").attr("required", true);
            $("#emailStar").show();
        } else {
            $("#Email").removeAttr("required");
            $("#emailStar").hide();
            $('[data-valmsg-for="Email"]').text('');
        }

        validateEmailField();
    });

    $('#Email').on('input', function () {
        var emailValue = $(this).val().trim();

        if (emailValue !== "") {
            $('#EmailNotAvailable').prop('checked', true);
        } else {
            $('#EmailNotAvailable').prop('checked', false);
        }

        $('#EmailNotAvailable').change(); // sync UI
        validateEmailField(); // 🔥 live validation
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

        var isDateValid = true;

        $('.vendor-datepicker').each(function () {

            var input = $(this);
            var value = input.val().trim();

            clearError(input);

            if (value === '') return;

            var regex = /^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/\d{4}$/;

            if (!regex.test(value)) {
                showError(input, "Invalid date format (dd/mm/yyyy)");
                isDateValid = false;
                return;
            }

            try {
                var parsedDate = $.datepicker.parseDate('dd/mm/yy', value);

                var day = ("0" + parsedDate.getDate()).slice(-2);
                var month = ("0" + (parsedDate.getMonth() + 1)).slice(-2);
                var year = parsedDate.getFullYear();

                var formatted = day + '/' + month + '/' + year;

                input.val(formatted);
                input.datepicker('setDate', parsedDate);

            } catch (e) {
                showError(input, "Invalid date");
                isDateValid = false;
            }
        });

        if (!isDateValid) return;


        var gstNo = $("#GSTRegNo").val();

        if (checkVendorGSTExists(gstNo)) {

            showToast("GST already exists!", "danger", 4000);
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
    
    $('#ApplicationMethod').val('1');
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

function validateEmailField() {
    var email = $('#Email').val().trim();
    var isChecked = $('#EmailNotAvailable').is(':checked');
    var errorSpan = $('[data-valmsg-for="Email"]');

    errorSpan.text(''); // clear old error

    if (isChecked) {

        if (email === "") {
            errorSpan.text("Email is required");
            return false;
        }

        if (email.toLowerCase().includes("italiagroup.in")) {
            errorSpan.text("italiagroup.in emails are not allowed");
            return false;
        }

        var emails = email.split(',');
        var regex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;

        for (var i = 0; i < emails.length; i++) {
            if (!regex.test(emails[i].trim())) {
                errorSpan.text("Invalid email format");
                return false;
            }
        }
    }

    return true;
}
function checkVendorGSTExists(gstNo) {

    var isExists = false;

    $.ajax({
        url: baseURL + 'Vendor/CheckVendorGSTRegistrationAlreadyExists',
        type: 'GET',
        data: { gstRegistrationNo: gstNo },
        async: false, // ⚠️ important (sync call)
        success: function (res) {
            isExists = res.data; // true / false
        },
        error: function () {
            isExists = false;
        }
    });

    return isExists;
}