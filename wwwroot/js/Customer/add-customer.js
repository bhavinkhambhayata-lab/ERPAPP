$(document).ready(function () {

    $("#Name").keyup(function () {

        var searchText = $(this).val();

        if (searchText.length < 2) {
            $("#customerSearchResult").html("");
            return;
        }

        var divisionId = $("#Division").val();

        if (!divisionId || divisionId === "0") {
          
            showToast("Please select first division", "danger", 4000);
            return;
        }

        $.ajax({
            url: '/Customer/SearchCustomer',
            type: 'GET',
            data: { searchText: searchText },
            success: function (data) {

                var html = "";

                $.each(data, function (i, item) {

                    html += "<a href='#' class='list-group-item list-group-item-action' data-no='"
                        + item.no + "'>" + item.name + "</a>";
                });

                $("#customerSearchResult").html(html);
            },
            error: function (err) {
                console.log("Error:", err);
            }
        });

    });

    $("#MasterCode").on("keyup", function (e) {

        var masterCode = $(this).val().trim();

        if (masterCode === "") return;

        // Enter press
        if (e.key === "Enter") {
            GetCustomerDataWithMasterCode(masterCode);
        }
    });

    $(document).on("focus", ".dlr-datepicker", function () {

        if (!$(this).hasClass("hasDatepicker")) {

            $(this).datepicker({
                dateFormat: "dd/mm/y",
                changeMonth: true,
                changeYear: true,

                onSelect: function (dateText) {

                    $(this).val(dateText);
                }

            });

        }

    });


    $(document).on("click", "#customerSearchResult a", function (e) {

        e.preventDefault();

        var masterCode = $(this).data("no");

        var divisionId = $("#Division").val();
        var divisionText = $("#Division option:selected").text();

        if (!divisionId || divisionId === "0") {
            showToast("Please select first division", "danger", 4000);
            return;
        }

        $("#Name").val($(this).text());
        $("#MasterCode").val(masterCode);
        $("#customerSearchResult").html("");

        // 🔥 Call API
        $.ajax({
            url: "/Customer/GetCustomerMasterData",
            type: "GET",
            data: { masterCode: masterCode, division: divisionText },
            success: function (response) {

                if (!response.success) {
                    showToast(response.message, "danger", 4000);
                    //$("#SaveBtn").prop("disabled", true);
                    return;
                }

                var data = response.data;

                loadCustomerAddressData(data.city, data.postcode)

                $("#Name").val(data.name);
                $("#Address").val(data.address);
                $("#Address2").val(data.address2);

                $("#CityCode").val(data.city);
                $("#PostCode").val(data.postcode);
                $("#StateCode").val(data.stateCode);
                $("#CountryCode").val(data.countryCode);
                $("#Region").val(data.region);
                $("#Zone").val(data.zone);

                $("#ContactPerson").val(data.contactPerson);
                $("#MobileNo").val(data.mobileNo);
                $("#PhoneNo").val(data.phoneNo);
                $("#FaxNo").val(data.faxNo);

                $("#Email").val(data.eMail);
                $("#EInvEmail").val(data.e_Inv_E_Mail);
                $("#EInvPhoneNo").val(data.e_Inv_PhoneNo);

                $("#Website").val(data.website_Homepage);

                $("#PANNo").val(data.panno);
                $("#BankName").val(data.bankName);
                $("#BankAccountNo").val(data.bankAccountNo);
                $("#BranchName").val(data.branchName);
                $("#IFSCCode").val(data.ifsCode);

                $("#GSTRegistrationNo").val(data.gstRegistrationNo);
                $("#GSTRegistrationType").val(data.gstRegistrationType);
                $("#GSTCustomerType").val(data.gstCustomerType);
                $("#CustomerType").val(data.customerType);

                $("#Allocation").val(data.allocation);

                $("#SaveBtn").prop("disabled", false);
            },
            error: function () {
                alert("Error loading customer data.");
            }
        });

    });

    //Empty city clears related fields
    $("#CityCode").on("input", function () {

        if ($(this).val().trim() === "") {

            $("#CountryCode").val('');
            $("#StateCode").val('');
            $("#PostCode").empty().append('<option value="">--Select--</option>');
            $("#Region").val('');
            $("#Zone").val('');

        }

    });

    // CITY AUTOCOMPLETE
    $("#CityCode").autocomplete({

        source: function (request, response) {

            $.get("/Customer/GetCityList",
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
            $("#Region").val('');
            $("#Zone").val('');

            // CITY DETAIL
            $.get("/Customer/GetCityDetail",
                { city: ui.item.value },
                function (data) {
                    console.log(data)
                    if (data) {
                        $("#CityCode").val(data.city);
                        $("#CountryCode").val(data.countryCode || '').trigger('change');;
                        $("#StateCode").val(data.stateCode || '');
                    }

                });

            // POSTCODE LIST
            $.get("/Customer/GetPostCodeList",
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

    // POSTCODE CHANGE
    $("#PostCode").change(function () {

        var postcode = $(this).val();

        // 🔥 CLEAR REGION & ZONE
        $("#Region").val('');
        $("#Zone").val('');

        if (!postcode) return;

        $.get("/Customer/GetPostCodeDetail",
            { postcode: postcode },
            function (data) {

                if (data) {
                    $("#Region").val(data.region || '');
                    $("#Zone").val(data.zone || '');
                }

            });

    });

    $('#btnSaveCustomerMaster').click(function () {

        var form = $('#customerForm');

        if (!form.valid()) return;

        if (!validateBrandTable()) return;

        // Brand row check
        if ($('#brandTable tbody tr').length == 0) {

            showToast("Please add at least one Brand.", "danger", 4000);

            return;
        }

        var formData = new FormData(form[0]);

        // disabled fields
        formData.append("DisplayNo", $('#DisplayNo').text());
        formData.append("MasterCode", $('#MasterCode').val());
        formData.append("StateCode", $('#StateCode').val());
        formData.append("Region", $('#Region').val());
        formData.append("Zone", $('#Zone').val());
        formData.append("BillToCustomer", $('#BillToCustomer').val());
        formData.append("DivisionCode", $('#Division option:selected').text());
        
        formData.append("CurrencyCode", $('#CurrencyCode').val());
        formData.append("CustomerPostingGroup", $('#CustomerPostingGroup').val());
        formData.append("GenBusPostingGroup", $('#GenBusPostingGroup').val());

        var isCheckedShipp = $("#chkSameAsGeneral").is(":checked");

        var isShippingEmpty =
            $('#ShippingName').val().trim() === '' &&
            $('#ShippingAddress').val().trim() === '' &&
            $('#ShippingCity').val().trim() === '';

        // ✅ CASE 1: Checked → Shipping field ni value j levani
        if (isCheckedShipp) {

            formData.set("ShippingName", $("#ShippingName").val());
            formData.set("ShippingAddress", $("#ShippingAddress").val());
            formData.set("ShippingAddress2", $("#ShippingAddress2").val());
            formData.set("ShippingCity", $("#ShippingCity").val());
            formData.set("ShippingPostalCode", $("#ShippingPostalCode").val());
            formData.set("ShippingState", $("#ShippingState").val());
            formData.set("ShippingCountry", $("#ShippingCountry").val());
            formData.set("ShippingPhoneNo", $("#ShippingPhoneNo").val());
            formData.set("ShippingEmail", $("#ShippingEmail").val());
            formData.set("ShippingContactPerson", $("#ShippingContactPerson").val());
            formData.set("ShippingLocationCode", $("#ShippingLocationCode").val());

            formData.set("ShippingAddressType", $("#ShippingAddressType").val());
        }

        // ✅ CASE 2: Unchecked + Empty → General values
        else if (isShippingEmpty) {

            formData.set("ShippingName", $("#Name").val());
            formData.set("ShippingAddress", $("#Address").val());
            formData.set("ShippingAddress2", $("#Address2").val());
            formData.set("ShippingCity", $("#CityCode").val());
            formData.set("ShippingPostalCode", $("#PostCode").val());
            formData.set("ShippingState", $("#StateCode").val());
            formData.set("ShippingCountry", $("#CountryCode").val());
            formData.set("ShippingPhoneNo", $("#PhoneNo").val());
            formData.set("ShippingEmail", $("#Email").val());
            formData.set("ShippingContactPerson", $("#ContactPerson").val());
            formData.set("ShippingLocationCode", $("#LocationCode").val());

            formData.set("ShippingAddressType", $("#ShippingAddressType").val());
        }

        // ✅ CASE 3: Unchecked + Manual → Shipping values
        else {

            formData.set("ShippingName", $("#ShippingName").val());
            formData.set("ShippingAddress", $("#ShippingAddress").val());
            formData.set("ShippingAddress2", $("#ShippingAddress2").val());
            formData.set("ShippingCity", $("#ShippingCity").val());
            formData.set("ShippingPostalCode", $("#ShippingPostalCode").val());
            formData.set("ShippingState", $("#ShippingState").val());
            formData.set("ShippingCountry", $("#ShippingCountry").val());
            formData.set("ShippingPhoneNo", $("#ShippingPhoneNo").val());
            formData.set("ShippingEmail", $("#ShippingEmail").val());
            formData.set("ShippingContactPerson", $("#ShippingContactPerson").val());
            formData.set("ShippingLocationCode", $("#ShippingLocationCode").val());

            formData.set("ShippingAddressType", $("#ShippingAddressType").val());
        }

        // ===== Brand List =====
        $('#brandTable tbody tr').each(function (index) {

            formData.append(`CustomerBrandAddList[${index}].CustomerNo`,
                $(this).find('[name*="CustomerNo"]').val());

            formData.append(`CustomerBrandAddList[${index}].BrandCode`,
                $(this).find('[name*="BrandCode"] option:selected').text());

            formData.append(`CustomerBrandAddList[${index}].CustomerCategoryCode`,
                $(this).find('[name*="CustomerCategoryCode"]').val());

            formData.append(`CustomerBrandAddList[${index}].TradeSecurityAmount`,
                $(this).find('[name*="TradeSecurityAmount"]').val());

            formData.append(`CustomerBrandAddList[${index}].CustomerDiscountGroup`,
                $(this).find('[name*="CustomerDiscountGroup"]').val());

            formData.append(`CustomerBrandAddList[${index}].DealerClassification`,
                $(this).find('[name*="DealerClassification"]').val());

            formData.append(`CustomerBrandAddList[${index}].SalesPersonCode`,
                $(this).find('[name*="SalesPersonCode"]').val());

            formData.append(`CustomerBrandAddList[${index}].Allocation`,
                $(this).find('[name*="Allocation"]').val());

            formData.append(`CustomerBrandAddList[${index}].HOSalesPerson`,
                $(this).find('[name*="HOSalesPerson"]').val());

            formData.append(`CustomerBrandAddList[${index}].DLRAppointmentDate`,
                $(this).find('[name*="DLRAppointmentDate"]').val());

            formData.append(`CustomerBrandAddList[${index}].DLRTerminationDate`,
                $(this).find('[name*="DLRTerminationDate"]').val());

        });

        $.ajax({
            url: '/Customer/SaveCustomerMaster',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {

                    //alert(response.message);
                    showToast("Customer Inserted Successfully.", "success", 4000);

                    $('#customerForm')[0].reset();

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

    $('#Division').change(function () {

        var divisionCode = $(this).val();

        // Brand Table Clear
        $('#brandTable tbody').empty();

        // 🔴 If division not selected → clear everything
        if (!divisionCode) {

            // Location
            $('#LocationCode').empty().append('<option value="">-- Select --</option>');
            $('#ShippingLocationCode').empty().append('<option value="">-- Select --</option>');

            // Price List
            $('#PriceListCode').empty().append('<option value="">-- Select --</option>');

            // Promo Code
            $('#PromoCode').empty().append('<option value="">-- Select --</option>');

            // Charges Group
            $('#ChargesGroup').empty().append('<option value="">-- Select --</option>');

            // Parent Customer Code
            $('#ParentCustomerCode').empty().append('<option value="">-- Select --</option>');

            return; // 🚀 stop further ajax calls
        }

        // 🔹 Location Call
        $.ajax({
            url: '/Customer/GetLocationListByDivisionCode',
            type: 'GET',
            data: { DivisionCode: divisionCode },
            success: function (data) {

                var locationDropdown = $('#LocationCode');
                locationDropdown.empty().append('<option value="">-- Select --</option>');

                $.each(data, function (i, item) {
                    locationDropdown.append(
                        '<option value="' + item.code + '">' + item.name + '</option>'
                    );
                });

                var shippinglocationDropdown = $('#ShippingLocationCode');
                shippinglocationDropdown.empty().append('<option value="">-- Select --</option>');

                $.each(data, function (i, item) {
                    shippinglocationDropdown.append(
                        '<option value="' + item.code + '">' + item.name + '</option>'
                    );
                });

                // ✅ Default selection logic
                if (divisionCode == 1) {
                    locationDropdown.val("FG-KAIYAL");
                    shippinglocationDropdown.val("FG-KAIYAL");
                }
                else if (divisionCode == 2) {
                    locationDropdown.val("FG-MOSAIC");
                    shippinglocationDropdown.val("FG-MOSAIC");
                }
            }
        });

        // 🔹 Division Wise Dropdown Call
        var divisionStr = $(this).find("option:selected").text();

        $.ajax({
            url: '/Customer/GetCustomerDivisionWiseDropDown',
            type: 'GET',
            data: { division: divisionStr },
            success: function (res) {
                debugger
                console.log(res); // debug

                // Price List
                var priceList = $('#PriceListCode');
                priceList.empty().append('<option value="">-- Select --</option>');

                if (res.priceList) {
                    $.each(res.priceList, function (i, item) {
                        priceList.append(
                            '<option value="' + item.code + '">' + item.name + '</option>'
                        );
                    });
                }

                // Promo Code
                var promoList = $('#PromoCode');
                promoList.empty().append('<option value="">-- Select --</option>');

                if (res.promoCodeList) {
                    $.each(res.promoCodeList, function (i, item) {
                        promoList.append(
                            '<option value="' + item.code + '">' + item.name + '</option>'
                        );
                    });
                }

                // Charges Group (FIX ID 👇)
                var chargesList = $('#ChargesGroup');
                chargesList.empty().append('<option value="">-- Select --</option>');

                if (res.chargesGroupList) {
                    $.each(res.chargesGroupList, function (i, item) {
                        chargesList.append(
                            '<option value="' + item.code + '">' + item.name + '</option>'
                        );
                    });
                }

                // Parent Customer Dropdown
                var parentCustomer = $('#ParentCustomerCode');
                parentCustomer.empty().append('<option value="">-- Select --</option>');

                if (res.parentCustomerList) {
                    $.each(res.parentCustomerList, function (i, item) {
                        parentCustomer.append(
                            '<option value="' + item.code + '">' + item.name + '</option>'
                        );
                    });
                }
            }
        });

    });

    $(document).on("change", "#brandTable tbody .brand", function () {

        var selectedValue = $(this).val();
        var selectedText = $(this).find("option:selected").text();
        var isDuplicate = false;

        $('#brandTable tbody .brand').not(this).each(function () {

            if ($(this).val() === selectedValue && selectedValue !== "") {
                isDuplicate = true;
            }

        });

        if (isDuplicate) {

            showToast("'" + selectedText + "' dimension is already added. Please select a different dimension.", "danger", 4000);

            $(this).val(""); // reset dropdown
        }

    });

    $("#btnFetchPortal").click(function () {

        var value = $("#PortalRowId").val().trim();
        var pattern = /^(ZZ\d+|\d+)$/i;

        // Empty check
        if (value === "") {
            $("#PortalRowIdError").text("");

            // Clear all fields
            $('#Division').val('').trigger('change');
            $("#Name").val("");
            $("#Address").val("");
            $("#Address2").val("");

            $("#CityCode").val("").trigger('change');
            $("#PostCode").empty().append('<option value="">--Select--</option>');
            $("#StateCode").val("");
            $("#CountryCode").val("");
            $("#Region").val("");
            $("#Zone").val("");

            $("#ContactPerson").val("");
            $("#Website").val("");
            $("#MobileNo").val("");
            $("#PhoneNo").val("");
            $("#Email").val("");

            $("#CustomerType").val("");
            $("#PANNo").val("");
            $("#GSTRegistrationNo").val("");
            $("#GSTRegistrationType").val("");

            $("#ARNNo").val("");

            return;
        }

        // Validation
        if (!pattern.test(value)) {
            $("#PortalRowIdError").text("Enter ZZ with digits or only digits.");
            return;
        } else {
            $("#PortalRowIdError").text("");
        }

        // API Call
        GetCustomerDataWithPortalRowId(value);

    });

    // Shipping Related Bind City AutoComplete

    //Empty city clears related fields
    $("#ShippingCity").on("input", function () {

        if ($(this).val().trim() === "") {

            $("#ShippingCity").val('');
            $("#ShippingState").val('');
            $("#ShippingPostalCode").empty().append('<option value="">--Select--</option>');

        }

    });

    // CITY AUTOCOMPLETE
    $("#ShippingCity").autocomplete({

        source: function (request, response) {

            $.get("/Customer/GetCityList",
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
            $("#ShippingCountry").val('');
            $("#ShippingState").val('');
            $("#ShippingPostalCode").empty().append('<option value="">--Select--</option>');

            // CITY DETAIL
            $.get("/Customer/GetCityDetail",
                { city: ui.item.value },
                function (data) {
                    console.log(data)
                    if (data) {
                        $("#ShippingCity").val(data.city);
                        $("#ShippingCountry").val(data.countryCode || '');
                        $("#ShippingState").val(data.stateCode || '');
                    }

                });

            // POSTCODE LIST
            $.get("/Customer/GetPostCodeList",
                { city: ui.item.value },
                function (data) {

                    $("#ShippingPostalCode").empty().append('<option value="">--Select--</option>');

                    $.each(data, function (i, item) {

                        $("#ShippingPostalCode").append(
                            '<option value="' + item.code + '">' + item.name + '</option>'
                        );

                    });

                });

            return false;
        }

    });

    $("#chkSameAsGeneral").change(function () {

        if ($(this).is(":checked")) {

            // ================= General → Shipping =================
            $("#ShippingName").val($("#Name").val()).prop("disabled", true);
            $("#ShippingAddress").val($("#Address").val()).prop("disabled", true);
            $("#ShippingAddress2").val($("#Address2").val()).prop("disabled", true);
            $("#ShippingCity").val($("#CityCode").val()).prop("disabled", true);
          
            $("#ShippingState").val($("#StateCode").val()).prop("disabled", true);
            $("#ShippingCountry").val($("#CountryCode").val()).prop("disabled", true);

            $("#ShippingPhoneNo").val($("#PhoneNo").val()).prop("disabled", true);
            $("#ShippingEmail").val($("#Email").val()).prop("disabled", true);
            $("#ShippingContactPerson").val($("#ContactPerson").val()).prop("disabled", true);

            $("#ShippingLocationCode").val($('#LocationCode').val()).prop("disabled", true);

            loadCustomerShippingAddressData($("#ShippingCity").val());
            
        } else {

            // ================= Clear =================
            $("#ShippingName").val("").prop("disabled", false);
            $("#ShippingAddress").val("").prop("disabled", false);
            $("#ShippingAddress2").val("").prop("disabled", false);
            $("#ShippingCity").val("").prop("disabled", false);
            $("#ShippingPostalCode").val("").prop("disabled", false);
            $("#ShippingState").val("").prop("disabled", false);
            $("#ShippingCountry").val("").prop("disabled", false);

            $("#ShippingPhoneNo").val("").prop("disabled", false);
            $("#ShippingEmail").val("").prop("disabled", false);
            $("#ShippingContactPerson").val("").prop("disabled", false);
        }
    });

    // On change
    $('#CommissionType').change(function () {
        handleCommission();
    });

    $('#CountryCode').on('change', function () {

        handleCurrency();
        handleCustomerPostingGroup();
        handleGenBusPostingGroup();
    });

    $(document).on('input', '.trade-security-amount', function () {
        var value = $(this).val();

        if (value && parseFloat(value) > 0) {
            $(this).val('0'); // blank kari devu
        }

        if (value && parseFloat(value) < 0) {
        }

        if (value == 0) {
        }
    });



    // Page load par pan run karvu
    handleCommission();


    $('#ShippingAddressType').val('0');
});


var maxBrandCount = 0;
function addBrandRow() {



    var divisionId = $("#Division").val();

    // Division selected check
    if (!divisionId) {

        showToast("Please select Division first.", "danger", 4000);
        return;
    }

    $.ajax({
        url: '/Customer/GetBrandRowDropdown',
        type: 'GET',
        data: { divisionRowId: divisionId },
        success: function (data) {

            // 🔥 Dimension count store
            maxBrandCount = data.dimensionList.length;

            var currentRows = $('#brandTable tbody tr').length;

            if (currentRows >= maxBrandCount) {
                showToast("You cannot add more brands for this division.", "danger", 4000);
                return;
            }

            createBrandRow(data);

        }
    });
}

function createBrandRow(data) {

    var table = document.getElementById("brandTable").getElementsByTagName('tbody')[0];

    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);

    var brandOptions = '<option value="">Select</option>';
    var categoryOptions = '<option value="">--Select--</option>';
    var discountOptions = '<option value="">--Select--</option>';
    var salesPersonOptions = '<option value="">--Select--</option>';
    var hoSalesPersonOptions = '<option value="">--Select--</option>';
    var dealerClassificationOptions = '<option value="">--Select--</option>';

    // Brand
    data.dimensionList.forEach(function (item) {
        brandOptions += `<option value="${item.code}">${item.name}</option>`;
    });

    // Customer Category
    data.customerCategoryList.forEach(function (item) {
        categoryOptions += `<option value="${item.code}">${item.name}</option>`;
    });

    // Discount Group
    data.discountGroupList.forEach(function (item) {
        discountOptions += `<option value="${item.code}">${item.name}</option>`;
    });

    // Sales Person (Allocation attach)
    data.salesPersonList.forEach(function (item) {
        salesPersonOptions += `<option value="${item.code}" data-allocation="${item.allocation}">
                                ${item.name}
                               </option>`;
    });

    // HO Sales Person
    data.hoSalesPersonList.forEach(function (item) {
        hoSalesPersonOptions += `<option value="${item.code}">${item.name}</option>`;
    });

    //Dealar Classification 
    data.dealerClassficationList.forEach(function (item) {
        dealerClassificationOptions += `<option value="${item.code}">${item.name}</option>`;
    });

    row.innerHTML = `
<td class='d-none'>
    <input name="CustomerList[${rowCount}].CustomerNo"
           class="form-control form-control-sm"/>
</td>

<td>
    <select name="CustomerList[${rowCount}].BrandCode"
            class="form-control form-control-sm brand">
        ${brandOptions}
    </select>
</td>

<td>
    <select name="CustomerList[${rowCount}].CustomerCategoryCode"
            class="form-control form-control-sm category">
        ${categoryOptions}
    </select>
</td>

<td>
    <select name="CustomerList[${rowCount}].CustomerDiscountGroup"
            class="form-control form-control-sm discount">
        ${discountOptions}
    </select>
</td>

<td>

 <select name="CustomerList[${rowCount}].DealerClassification"
            class="form-control form-control-sm dealerClassfication">
        ${dealerClassificationOptions}
    </select>
</td>

<td>
    <select name="CustomerList[${rowCount}].SalesPersonCode"
            class="form-control form-control-sm sales"
            onchange="setAllocation(this)">
        ${salesPersonOptions}
    </select>
</td>

<td>
    <input name="CustomerList[${rowCount}].Allocation"
           class="form-control form-control-sm allocation"
           readonly/>
</td>

<td>
    <select name="CustomerList[${rowCount}].HOSalesPerson"
            class="form-control form-control-sm ho">
        ${hoSalesPersonOptions}
    </select>
</td>

<td>
    <input type="text"
           name="CustomerList[${rowCount}].DLRAppointmentDate"
           class="form-control form-control-sm dlr-datepicker"/>
</td>

<td>
    <input type="text"
           name="CustomerList[${rowCount}].DLRTerminationDate"
           class="form-control form-control-sm dlr-datepicker" disabled/>
</td>

<td>
    <input name="CustomerList[${rowCount}].TradeSecurityAmount"
           type="number"
           class="form-control form-control-sm text-end trade-security-amount" value="0" max="0" />
</td>

<td class="text-center">
    <button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">
        ✕
    </button>
</td>
`;
}

function setAllocation(element) {

    var allocation = element.options[element.selectedIndex].getAttribute("data-allocation");

    var row = element.closest("tr");
    var allocationInput = row.querySelector(".allocation");

    allocationInput.value = allocation || "";

}

function removeRow(button) {
    var row = button.closest("tr");
    row.remove();
    reIndexRows();
}

function reIndexRows() {
    var rows = document.querySelectorAll("#brandTable tbody tr");

    rows.forEach(function (row, index) {
        row.querySelectorAll("input").forEach(function (input) {
            var name = input.getAttribute("name");
            if (name) {
                input.setAttribute("name",
                    name.replace(/\[\d+\]/, "[" + index + "]"));
            }
        });
    });
}

function validateBrandTable() {

    var isValid = true;

    $('#brandTable tbody tr').each(function () {

        $(this).find('select').removeClass('is-invalid');

        if ($(this).find('.brand').val() == "") {
            $(this).find('.brand').addClass('is-invalid');
            isValid = false;
        }

        if ($(this).find('.category').val() == "") {
            $(this).find('.category').addClass('is-invalid');
            isValid = false;
        }

        if ($(this).find('.discount').val() == "") {
            $(this).find('.discount').addClass('is-invalid');
            isValid = false;
        }

        if ($(this).find('.sales').val() == "") {
            $(this).find('.sales').addClass('is-invalid');
            isValid = false;
        }

        if ($(this).find('.ho').val() == "") {
            $(this).find('.ho').addClass('is-invalid');
            isValid = false;
        }

    });

    return isValid;
}

function GetCustomerDataWithPortalRowId(value) {


    var portalRowId = value.replace(/^zz/i, '');

    if (!portalRowId) {
        $("#PortalRowIdError").text("Please enter Portal Row Id");
        return;
    }

    $("#PortalRowIdError").text("");

    $.ajax({
        url: '/Customer/GetCustomerDataWithPortalRowId',
        type: 'GET',
        data: { portalRowId: portalRowId },
        success: function (res) {

            if (!res.success) {
                $("#PortalRowIdError").text(res.message);
                return;
            }

            var data = res.result;

            //$("#MasterCode").val(data.masterCode);

            //First Default Select MOSAIC because that is getting bella 
            $('#Division').val('2').trigger('change');

            $("#Name").val(data.name);
            $("#Address").val(data.address);
            $("#Address2").val(data.address2);
            $("#CityCode").val(data.cityCode);

            loadCustomerAddressData(data.cityCode, data.postCode);

            $("#PostCode").val(data.postCode);
            $("#StateCode").val(data.stateCode);
            $("#CountryCode").val(data.countryCode).trigger('change');

            $("#Region").val(data.region);
            $("#Zone").val(data.zone);

            $("#ContactPerson").val(data.contactPerson);
            $("#MobileNo").val(data.mobileNo);
            $("#PhoneNo").val(data.phoneNo);

            $("#Email").val(data.email);
            $("#Website").val(data.website);

            $("#CustomerType").val(data.customerType);

            $("#PANNo").val(data.panNo);

            $("#GSTRegistrationNo").val(data.gstRegistrationNo);
            $("#GSTRegistrationType").val(data.gstRegistrationType);

            $("#ARNNo").val(data.arnNo);

        },
        error: function () {

        }
    });

}


function loadCustomerAddressData(city,postcode) {
    // 🔥 CLEAR OLD DATA
    $("#CountryCode").val('');
    $("#StateCode").val('');
    $("#PostCode").empty().append('<option value="">--Select--</option>');
    $("#Region").val('');
    $("#Zone").val('');

    // CITY DETAIL
    $.get("/Customer/GetCityDetail",
        { city: city },
        function (data) {

            if (data) {
                $("#CityCode").val(data.city);
                $("#CountryCode").val(data.countryCode || '');
                $("#StateCode").val(data.stateCode || '');
            }

        });

    // POSTCODE LIST
    $.get("/Customer/GetPostCodeList",
        { city: city },
        function (data) {

            $("#PostCode").empty().append('<option value="">--Select--</option>');

            $.each(data, function (i, item) {

                $("#PostCode").append(
                    '<option value="' + item.code + '">' + item.name + '</option>'
                );

            });

            $("#PostCode").val(postcode);


        });
}

function GetCustomerDataWithMasterCode(masterCode) {

    $.ajax({
        url: '/Customer/GetCustomerDataWithMasterCode',
        type: 'GET',
        data: { masterCode: masterCode },

        success: function (res) {

            if (!res.success) {
                showToast(res.message, "warning", 4000);
                return;
            }

            var data = res.result;

            $("#Name").val(data.name);
            $("#Address").val(data.address);
            $("#Address2").val(data.address2);

            $("#CityCode").val(data.city);
            $("#PostCode").val(data.postcode);

            $("#StateCode").val(data.stateCode);
            $("#CountryCode").val(data.countryCode);
            $("#Region").val(data.region);
            $("#Zone").val(data.zone);

            $("#ContactPerson").val(data.contactPerson);
            $("#MobileNo").val(data.mobileNo);
            $("#PhoneNo").val(data.phoneNo);
            $("#FaxNo").val(data.faxNo);

            $("#Email").val(data.eMail);
            $("#EInvEmail").val(data.e_Inv_E_Mail);
            $("#EInvPhoneNo").val(data.e_Inv_PhoneNo);

            $("#Website").val(data.website_Homepage);

            $("#PANNo").val(data.panno);

            $("#BankName").val(data.bankName);
            $("#BankAccountNo").val(data.bankAccountNo);
            $("#BranchName").val(data.branchName);
            $("#IFSCCode").val(data.ifsCode);

            $("#GSTRegistrationNo").val(data.gstRegistrationNo);
            $("#GSTRegistrationType").val(data.gstRegistrationType);
            $("#GSTCustomerType").val(data.gstCustomerType);

            $("#CustomerType").val(data.customerType);

            $("#Allocation").val(data.allocation);
        },

        error: function () {

            showToast("Error loading customer data.", "danger", 4000);

        }
    });

}
function loadCustomerShippingAddressData(city) {
    // 🔥 CLEAR OLD DATA
    $("#ShippingCountry").val('');
    $("#ShippingState").val('');
    $("#ShippingPostalCode").empty().append('<option value="">--Select--</option>');


    // CITY DETAIL
    $.get("/Customer/GetCityDetail",
        { city: city },
        function (data) {

            if (data) {
                $("#ShippingCity").val(data.city);
                $("#ShippingCountry").val(data.countryCode || '');
                $("#ShippingState").val(data.stateCode || '');
            }

        });

    // POSTCODE LIST
    $.get("/Customer/GetPostCodeList",
        { city: city },
        function (data) {

            $("#ShippingPostalCode").empty().append('<option value="">--Select--</option>');

            $.each(data, function (i, item) {

                $("#ShippingPostalCode").append(
                    '<option value="' + item.code + '">' + item.name + '</option>'
                );

            });
            // 👉 Ahiya select karavvu
            var postCode = $("#PostCode").val();

            $("#ShippingPostalCode")
                .val(postCode)
                .prop("disabled", true);
        });

}

function handleCommission() {
    var value = $('#CommissionType').val();

    var commissionDiv = $('.commission-labels').closest('.row');
    var label = $('.commission-labels');

    if (value === "1") {
        // Differential → hide
        commissionDiv.addClass('d-none');
    }
    else if (value === "2") {
        // PercentOfNetRealization
        commissionDiv.removeClass('d-none');
        label.text('Commission % of Net');
    }
    else if (value === "3") {
        // QtyPerUOM
        commissionDiv.removeClass('d-none');
        label.text('Commission Per Unit');
    }
    else {
        // None or empty
        commissionDiv.addClass('d-none');
    }
}
function handleCurrency() {
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

function handleCustomerPostingGroup() {

    var country = $('#CountryCode').val();
    var postingGroup = $('#CustomerPostingGroup');

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

function handleGenBusPostingGroup() {

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