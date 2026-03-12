$(document).ready(function () {

    $("#Name").keyup(function () {

        var searchText = $(this).val();

        if (searchText.length < 2) {
            $("#customerSearchResult").html("");
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

        var customerNo = $(this).data("no");
        var brandId = $("#Brand").val();

        $("#Name").val($(this).text());
        $("#CustomerNo").val(customerNo);
        $("#customerSearchResult").html("");

        // 🔥 Call API
        $.ajax({
            url: "/Customer/GetCustomerMaster",
            type: "GET",
            data: { customerNo: customerNo, brandId: brandId },
            success: function (response) {

                if (!response.success) {
                    showToast(response.message, "warning", 4000);
                    $("#SaveBtn").prop("disabled", true);
                    return;
                }

                var data = response.data;

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
                        $("#CountryCode").val(data.countryCode || '');
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

        // ===== Brand List =====
        $('#brandTable tbody tr').each(function (index) {

            formData.append(`CustomerBrandAddList[${index}].CustomerNo`,
                $(this).find('[name*="CustomerNo"]').val());

            formData.append(`CustomerBrandAddList[${index}].BrandCode`,
                $(this).find('[name*="BrandCode"]').val());

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

                    alert(response.message);

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

        $.ajax({
            url: '/Customer/GetLocationListByDivisionCode',
            type: 'GET',
            data: { DivisionCode: divisionCode },
            success: function (data) {

                var locationDropdown = $('#LocationCode');
                locationDropdown.empty();

                locationDropdown.append('<option value="">-- Select --</option>');

                $.each(data, function (i, item) {
                    locationDropdown.append(
                        '<option value="' + item.code + '">' + item.name + '</option>'
                    );
                });
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

            showToast("'"+selectedText + "' dimension is already added. Please select a different dimension.", "danger", 4000);

            $(this).val(""); // reset dropdown
        }

    });

    $("#PortalRowId").on("keyup", function (e) {

        var value = $(this).val();
        var pattern = /^(ZZ\d+|\d+)$/i;

        if (value === "") {
            $("#PortalRowIdError").text("");

            // Clear all fields
            $("#Name").val("");
            $("#Address").val("");
            $("#Address2").val("");
            $("#CityCode").val("");
            $("#PostCode").val("");
            $("#StateCode").val("");
            $("#CountryCode").val("");
            $("#Region").val("");
            $("#Zone").val("");

            $("#ContactPerson").val("");
            $("#MobileNo").val("");
            $("#Email").val("");
            return;
        }

        if (!pattern.test(value)) {
            $("#PortalRowIdError").text("Enter ZZ with digits or only digits.");
        } else {
            $("#PortalRowIdError").text("");
        }

        // Enter press
        if (e.key === "Enter") {

            if (pattern.test(value)) {
                GetCustomerDataWithPortalRowId(value);   // function call
            }

        }
    });
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
<td>
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
    <input name="CustomerList[${rowCount}].TradeSecurityAmount"
           type="number"
           class="form-control form-control-sm text-end" value="0"/>
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
           class="form-control form-control-sm dlr-datepicker"/>
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

            $("#Name").val(data.name);
            $("#Address").val(data.address);
            $("#Address2").val(data.address2);
            $("#CityCode").val(data.cityCode);
            $("#PostCode").val(data.postCode);
            $("#StateCode").val(data.stateCode);
            $("#CountryCode").val(data.countryCode);
            $("#Region").val(data.region);
            $("#Zone").val(data.zone);

            $("#ContactPerson").val(data.contactPerson);
            $("#MobileNo").val(data.mobileNo);
            $("#Email").val(data.email);
        },
        error: function () {
            
        }
    });

}