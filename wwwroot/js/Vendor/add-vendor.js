$(document).ready(function () {

    $('.searchable-dropdown').select2({
        theme: "bootstrap-5",   // 🔥 IMPORTANT
        placeholder: "--Select--",
        allowClear: true,
        width: '100%'
    });

    $("#Name").keyup(function () {

        var searchText = $(this).val();

        if (searchText.length < 2) {
            $("#vendorSearchResult").html("");
            return;
        }

        $.ajax({
            url: baseURL + 'Vendor/SearchVendor',
            type: 'GET',
            data: { searchVendor: searchText },
            success: function (data) {

                var html = "";

                $.each(data, function (i, item) {

                    html += "<a href='#' class='list-group-item list-group-item-action' data-no='"
                        + item.code + "'>" + item.name + "</a>";
                });

                $("#vendorSearchResult").html(html);
            },
            error: function (err) {
                console.log("Error:", err);
            }
        });

    });

    $(document).on("click", "#vendorSearchResult a", function (e) {

        e.preventDefault();

        var masterCode = $(this).data("no");

        $("#Name").val($(this).text());
        $("#MasterCode").val(masterCode);
        $("#vendorSearchResult").html("");

        // 🔥 Call API
        $.ajax({
            url: baseURL + "Vendor/GetVendorMasterDataWithMasterCode",
            type: "GET",
            data: { masterCode: masterCode },
            success: function (response) {

                if (!response.success) {
                    showToast(response.message, "danger", 4000);
                    return;
                }

                var data = response.data;

                // OPTIONAL
                loadCustomerAddressData(data.cityCode, data.postCode);

                // ================= BASIC =================
                $("#MasterCode").val(data.masterCode);

                $("#Name").val(data.name || '');
                $("#Address").val(data.address || '');
                $("#Address2").val(data.address2 || '');

                $("#CityCode").val(data.cityCode || '');
                $("#PostCode").val(data.postCode || '').trigger("change");
                $("#StateCode").val(data.stateCode || '');
                $("#CountryCode").val(data.countryCode || '').trigger("change");

                // ================= CONTACT =================
                $("#ContactPerson").val(data.contactPerson || '');
                $("#MobileNo").val(data.mobileNo || '');
                $("#PhoneNo").val(data.phoneNo || '');
                $("#Email").val(data.email || '');

                $("#EmailNotAvailable").prop("checked", data.emailNotAvailable === true || data.emailNotAvailable === "true").trigger("change");

                $("#Website").val(data.website || '');

                // ================= TAX =================
                $("#PANNo").val(data.panNo || '');
                //$("#CurrencyCode").val(data.currencyCode || '').trigger("change");

                // ================= GST =================
                $("#GSTVendorType").val(data.gstVendorType || '').trigger("change");
                $("#GSTReturnFrequency").val(data.gstReturnFrequency || '').trigger("change");
                $("#GSTRegNo").val(data.gstRegNo || '');
                $("#ARN").val(data.arn || '');

                // ================= BANK =================
                $("#BankName").val(data.bankName || '');
                $("#BankAccountNo").val(data.bankAccountNo || '');
                $("#BranchName").val(data.branchName || '');
                $("#IFSCCode").val(data.ifscCode || '');

                // ================= BUSINESS =================
                $("#VendorCategory").val(data.vendorCategory || '').trigger("change");
                $("#BusinessCategory").val(data.businessCategory || '').trigger("change");

                $("#RelatedParty").val(data.relatedParty === true || data.relatedParty === "true" ? "true" : "false");

                $("#Subcontractor").val(data.subcontractor === true || data.subcontractor === "true" ? "true" : "false");

                $("#PaymentTerms").val(data.paymentTerms || '').trigger("change");
                $("#PaymentMethod").val(data.paymentMethod || '').trigger("change");
                $("#PurchaserCode").val(data.purchaserCode || '').trigger("change");

                // ================= POSTING =================
                //$("#VATBusPostingGroup").val(data.vatBusPostingGroup || '').trigger("change");
                //$("#GenBusPostingGroup").val(data.genBusPostingGroup || '').trigger("change");
                //$("#VendorPostingGroup").val(data.vendorPostingGroup || '').trigger("change");

                // ================= OTHER =================
                $("#ApplicationMethod").val(data.applicationMethod || '').trigger("change");
                $("#TaxLiable").val(data.taxLiable || '').trigger("change");
                $("#Location").val(data.location || '').trigger("change");

                // ================= MSME =================
                $("#MSMEUAMNo").val(data.msmeuamNo || '');

                // Date format (if needed)
                $("#MSMEIntimationDate").val(editVendorFormatDateOrEmpty(data.msmeIntimationDate));
                $("#MSMEEffectiveDate").val(editVendorFormatDateOrEmpty(data.msmeEffectiveDate || ''));

                // ================= EXTRA =================
                $("#VendorLocation").val(data.vendorLocation || '').trigger("change");
                $("#GSTNotToHold").val(data.gstNotToHold ?? '').trigger("change");
                $("#FixedDueDate").val(data.fixedDueDate ?? '').trigger("change");
                //$("#AggregateTurnover").val(data.aggregateTurnover || '').trigger("change");

            },
            error: function () {
                alert("Error loading vendor data.");
            }
        });

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

        formData.append("Email", $("#Email").val());
        formData.append("GSTRegNo", $("#GSTRegNo").val());
        formData.append("ARN", $("#ARN").val());
        formData.append("GSTReturnFrequency", $("#GSTReturnFrequency").val());

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

    var emails = email.split(',');
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