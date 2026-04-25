$(document).ready(function () {

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

            $("#Email").prop("disabled", true).val('');
            $("#Email").removeAttr("required");

            $("#emailStar").hide();
            $('[data-valmsg-for="Email"]').text('');

        } else {

            $("#Email").prop("disabled", false);
            $("#Email").attr("required", true);
            $("#emailStar").show();
        }

        validateEmailField();
    });

    //Empty city clears related fields
    $("#CityCode").on("input", function () {

        if ($(this).val().trim() === "") {

            // 🔥 Clear City hidden
            $("#CityCode").val('');
            $("#PostCode").val('');

            $("#CountryCode").val('');
            $("#StateCode").val('');
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

            return false;
        }

    });

    $("#PostCode").autocomplete({

        source: function (request, response) {

            $.ajax({
                url: baseURL + "Vendor/GetPostCodeListWithSearch",
                type: "GET",
                dataType: "json",
                data: {
                    city: $('#CityCode').val(),
                    search: request.term   // 🔥 search text
                },
                success: function (data) {

                    response($.map(data, function (item) {
                        return {
                            label: item.name,   // show in dropdown
                            value: item.name,   // set in textbox
                            code: item.code     // hidden field mate
                        };
                    }));

                }
            });

        },

        minLength: 1,

        select: function (event, ui) {

            $("#PostCode").val(ui.item.code);

            return false;
        }
    });

    $("#CountryCode").autocomplete({
        source: function (request, response) {

            $.get(baseURL + "Vendor/GetCountryList", {
                searchcountry: request.term   // 🔥 Name પરથી search
            }, function (data) {

                response($.map(data, function (item) {
                    return {
                        label: item.name,   // display → "Code - Name"
                        value: item.code    // textbox માં → Code set થશે
                    };
                }));

            });
        },

        minLength: 1,

        select: function (event, ui) {

            // 🔥 textbox માં Code set થશે
            $("#CountryCode").val(ui.item.value);

            // 🔥 change event manually trigger
            $("#CountryCode").trigger("change");

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

                // Existing logic
                $("#AggregateTurnover").prop("selectedIndex", 2);
                $("#GSTReturnFrequency").prop("selectedIndex", 0);

                // ✅ Disable fields
                $("#GSTRegNo").prop("disabled", true).val('');
                $("#ARN").prop("disabled", true).val('');
                $("#GSTReturnFrequency").prop("disabled", true);

            } else {

                // Existing logic
                $("#AggregateTurnover").prop("selectedIndex", 1);

                // ✅ Enable fields
                $("#GSTRegNo").prop("disabled", false);
                $("#ARN").prop("disabled", false);
                $("#GSTReturnFrequency").prop("disabled", false);
            }
        }
    });

    $('#btnUpdateVendorMaster').click(function () {

        var form = $('#vendorUnBlockEditForm');

        if (!form.valid()) return;

        var formData = new FormData(form[0]);

        formData.append("Name", $("#Name").val());
        formData.append("Address", $("#Address").val());
        formData.append("Address2", $("#Address2").val());
        formData.append("CityCode", $("#CityCode").val());
        formData.append("PostCode", $("#PostCode").val());
        formData.append("CountryCode", $("#CountryCode").val());
        formData.append("ContactPerson", $("#ContactPerson").val());
        formData.append("MobileNo", $("#MobileNo").val());
        formData.append("PhoneNo", $("#PhoneNo").val());
        formData.append("Email", $("#Email").val());
        formData.set("EmailNotAvailable", $("#EmailNotAvailable").is(":checked"));
        formData.append("Website", $("#Website").val());
        formData.append("PANNo", $("#PANNo").val());
        formData.append("AssesseeCode", $("#AssesseeCode").val());
        formData.append("BankName", $("#BankName").val());
        formData.append("BankAccountNo", $("#BankAccountNo").val());
        formData.append("BranchName", $("#BranchName").val());
        formData.append("IFSCCode", $("#IFSCCode").val());
        formData.append("BusinessCategory", $("#BusinessCategory").val());


        // disabled fields
        formData.append("AggregateTurnover", $('#AggregateTurnover').val());
        formData.append("StateCode", $('#StateCode').val());
        formData.append("DisplayNo", $('#DisplayNo').text());
        formData.append("MasterCode", $('#MasterCode').val());
        formData.append("CompanyCode", $('#CompanyCode').val());
        formData.append("CurrencyCode", $('#CurrencyCode').val());
        formData.append("VendorPostingGroup", $('#VendorPostingGroup').val());
        formData.append("GenBusPostingGroup", $('#GenBusPostingGroup').val());

        formData.append("Email", $("#Email").val());
        formData.append("GSTRegNo", $("#GSTRegNo").val());
        formData.append("ARN", $("#ARN").val());
        formData.append("GSTReturnFrequency", $("#GSTReturnFrequency").val());

        formData.append("IsAlreadyCreatedMaster", $("#IsAlreadyCreatedMaster").val());

        formData.append("MSMEUAMNo", $("#MSMEUAMNo").val());
        formData.append("MSMEEffectiveDate", $("#MSMEEffectiveDate").val());
        formData.append("MSMEIntimationDate", $("#MSMEIntimationDate").val());

        formData.append("GSTVendorType", $("#GSTVendorType").val());
       

        // 👉 Email + Checkbox validation
        var vendorEmail = $('#Email').val().trim();
        var isCheckedEmailNoAvailable = $('#EmailNotAvailable').is(':checked');

        // ✅ Required only when checkbox NOT checked
        if (!isCheckedEmailNoAvailable && vendorEmail === "") {
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


        //var gstNo = $("#GSTRegNo").val();

        //if (checkVendorGSTExists(gstNo)) {

        //    showToast("GST already exists!", "danger", 4000);
        //    return;
        //}

        showLoader();

        $.ajax({
            url: baseURL + 'Vendor/UpdateVendorMaster',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {

                    hideLoader();

                    //alert(response.message);
                    showToast("Vendor Updated Successfully.", "success", 4000);

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

                        hideLoader();

                    } else {

                        alert(response.message);
                        hideLoader();
                    }

                }

            }
        });
    });

    //$('#ApplicationMethod').val('0');

    $('#PANNo').on('keyup', function () {
        handleVendorPANChange($(this).val());
    });
    handleVendorPANChange($("#PANNo").val());

    $("#EmailNotAvailable").trigger("change");
    $('#CountryCode').trigger('change');
    $('#GSTVendorType').trigger('change');

    loadVendorNonEditableFields();
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
        genBusPostingGroup.val("IMPORT");
        genBusPostingGroup.prop("disabled", true);
    }
}

function validateEmailField() {
    
    var email = $('#Email').val().trim();
    var isChecked = $('#EmailNotAvailable').is(':checked');
    var errorSpan = $('[data-valmsg-for="Email"]');

    errorSpan.text('');

    if (isChecked) {
        return true;
    }

    if (email === "") {
        errorSpan.text("Email is required");
        return false;
    }

    if (email.toLowerCase().includes("italiagroup.in")) {
        errorSpan.text("italiagroup.in emails are not allowed");
        return false;
    }

    var emails = email.split(';');
    var regex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;

    for (var i = 0; i < emails.length; i++) {
        if (!regex.test(emails[i].trim())) {
            errorSpan.text("Invalid email format");
            return false;
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
function editVendorFormatDateOrEmpty(date) {
    if (!date) return '';

    var d = new Date(date);

    // Invalid or SQL min date (1753)
    if (isNaN(d) || d.getFullYear() === 1753) return '';

    var day = String(d.getDate()).padStart(2, '0');
    var month = String(d.getMonth() + 1).padStart(2, '0');
    var year = d.getFullYear();

    return day + '/' + month + '/' + year;
}
function handleVendorPANChange(panNo) {

    panNo = panNo.toUpperCase();
    $("#PANNo").val(panNo);

    var panRegex = /^[A-Z]{5}[0-9]{4}[A-Z]$/;

    if (panNo.length === 10 && panRegex.test(panNo)) {

        var place = panNo.substring(3, 4);

        $.ajax({
            url: baseURL + 'Vendor/GetAssessCodeWithPlace',
            type: 'GET',
            data: { Place: place },
            success: function (res) {
                $("#AssesseeCode").val(res?.code || '');
            }
        });

    } else {
        $("#AssesseeCode").val('');
    }
}

function loadVendorNonEditableFields() {
    //Static
    //$("#IsAlreadyCreatedMaster").val("0");
    var isAlreadyCreatedMaster = $("#IsAlreadyCreatedMaster").val();

    if (isAlreadyCreatedMaster == "1") {

        // BASIC
        $("#Name").prop("disabled", true);
        $("#Address").prop("disabled", true);
        $("#Address2").prop("disabled", true);
        $("#CityCode").prop("disabled", true);
        $("#PostCode").prop("disabled", true);
        $("#StateCode").prop("disabled", true);
        $("#CountryCode").prop("disabled", true);

        $("#BusinessCategory").prop("disabled", true);

        // MSME
        $("#MSMEUAMNo").prop("disabled", true);
        $("#MSMEEffectiveDate").prop("disabled", true);
        $("#MSMEIntimationDate").prop("disabled", true);

        // CONTACT
        $("#ContactPerson").prop("disabled", true);
        $("#MobileNo").prop("disabled", true);
        $("#EmailNotAvailable").prop("disabled", true);
        $("#Email").prop("disabled", true);
        $("#PhoneNo").prop("disabled", true);
        $("#Website").prop("disabled", true);

        // TAX
        $("#PANNo").prop("disabled", true);
        $("#AssesseeCode").prop("disabled", true);

        // GST
        $("#GSTVendorType").prop("disabled", true);
        $("#AggregateTurnover").prop("disabled", true);
        $("#GSTRegNo").prop("disabled", true);
        $("#ARN").prop("disabled", true);
        $("#GSTReturnFrequency").prop("disabled", true);

        // BANK
        $("#BankName").prop("disabled", true);
        $("#BankAccountNo").prop("disabled", true);
        $("#BranchName").prop("disabled", true);
        $("#IFSCCode").prop("disabled", true);
    }
}

function toggleVendorUnBlock(btn) {

    var vendorCode = btn.getAttribute("data-vendor");

    if (!confirm("Are you sure you want to unblock this vendor?")) {
        return; // ❌ cancel
    }

    showLoader();

    $.ajax({
        url: baseURL + 'Vendor/VendorUnblock',
        type: 'POST',
        data: {
            vendorCode: vendorCode,
            displayRowId: $("#DisplayNo").text(),
            vendorName: $("#Name").val(),
            locationName: $("#Location").val()
        },
        success: function (res) {

            if (res.success) {

                hideLoader();
                showToast(res.message, "success", 3000);

                btn.style.display = "none";

                

            } else {
                alert(res.message || "Something went wrong!");
            }
        }
    });
}