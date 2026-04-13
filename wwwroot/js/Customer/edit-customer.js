$(document).ready(function () {

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

    $(document).on("change", ".brand", function () {

        var current = $(this);

        var currentText = current.find("option:selected").text().trim();

        if (!currentText || currentText === "Select") return;

        var brandList = [];

        // ✅ Hidden inputs (jo text store hoy to)
        $("#brandTable tbody input[name*='BrandCode']").each(function () {
            var val = $(this).val();
            if (val) {
                brandList.push(val.trim().toUpperCase());
            }
        });

        // ✅ Other dropdowns mathi TEXT lo (except current)
        $("#brandTable tbody .brand").not(current).each(function () {

            var txt = $(this).find("option:selected").text().trim();

            if (txt && txt !== "Select") {
                brandList.push(txt.toUpperCase());
            }

        });

        // ✅ Duplicate check (TEXT compare)
        if (brandList.includes(currentText.toUpperCase())) {

            showToast("'" + currentText + "' already added!", "danger", 4000);

            current.val("").trigger("change");
            return;
        }

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


    $("#btnSaveCustomerMaster").click(function () {

        if (!validateBrandTable()) return;

        if ($("#brandTable tbody tr").filter(function () {
            return $(this).find(".is-edit-cls").val() == "false";
        }).length === 0) {

            showToast("Please add at least one Brand.", "danger", 4000);
            return;
        }

        var list = [];

        $("#brandTable tbody tr").each(function () {

            var row = $(this);

            // ✅ Only take IsEdited = false
            if (row.find(".is-edit-cls").val() !== "false") return;

            var item = {
                CustomerNo: $('#BillToCustomer').val(),
                BrandCode: row.find("[name*='BrandCode'] option:selected").text() || "",
                CustomerCategoryCode: row.find("[name*='CustomerCategoryCode']").val() || "",
                TradeSecurityAmount: parseFloat(row.find("[name*='TradeSecurityAmount']").val()) || 0,
                CustomerDiscountGroup: row.find("[name*='CustomerDiscountGroup']").val() || "",
                DealerClassification: row.find("[name*='DealerClassification']").val() || "",
                SalesPersonCode: row.find("[name*='SalesPersonCode']").val() || "",
                Allocation: row.find("[name*='Allocation']").val() || "",
                HOSalesPerson: row.find("[name*='HOSalesPerson']").val() || "",
                DLRAppointmentDate: formatDate(row.find("[name*='DLRAppointmentDate']").val()),
                DLRTerminationDate: formatDate(row.find("[name*='DLRTerminationDate']").val()),
                IsEdited: false
            };

            list.push(item);
        });


        $.ajax({
            url: baseURL + 'Customer/EditCustomerDetailsBrandWiseDataOnly',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(list),
            success: function (res) {
                if (res.success) {
                    showToast(res.message, "success", 4000);
                    $('#listBtn').click();

                } else {
                    showToast(res.message, "danger", 4000);
                }
            }
        });

    });

});

function formatDate(val) {
    if (!val) return null;

    var parts = val.split('/');

    // "21/03/26" → dd/MM/yy
    if (parts.length === 3) {
        var day = parts[0];
        var month = parts[1];
        var year = parts[2];

        // 2 digit year → 20xx
        if (year.length === 2) {
            year = "20" + year;
        }

        return `${year}-${month}-${day}`; // yyyy-MM-dd
    }

    return null;
}

var maxBrandCount = 0;
function addBrandRow() {

    var divisionId = $("#Division").val();
    var customerType = $("#CustomerType").val();

    // Division selected check
    if (!divisionId) {

        showToast("Please select Division first.", "danger", 4000);
        return;
    }

    // Customer Type selected check
    if (!customerType) {

        showToast("Please select Customer Type.", "danger", 4000);
        return;
    }

    $.ajax({
        url: baseURL + 'Customer/GetBrandRowDropdown',
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

    var customerType = $("#CustomerType").val();

    // Customer Category
    data.customerCategoryList.forEach(function (item) {
        var typePrefix = item.code.split('.')[0]; // 1 / 2 / 3

        if (typePrefix === customerType) {
            categoryOptions += `<option value="${item.code}">${item.name}</option>`;
        }
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

 <select name="CustomerList[${rowCount}].DealerClassification"
            class="form-control form-control-sm dealerClassfication">
        ${dealerClassificationOptions}
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
           class="form-control form-control-sm text-end trade-security-amount" value="0.00" disabled/>
</td>

<td class="text-center">
    <button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">
        ✕
    </button>
</td>
 <td class="d-none">
     <input type="text" name="CustomerBrandEditList[@i].IsEdited" value="false" class="form-control is-edit-cls" />
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
function handlePriceListCode() {

    var country = $('#CountryCode').val();
    var division = $('#Division option:selected').text();
    var priceListCode = $('#PriceListCode');

    if (!country || !division) return;

    // MOSAIC
    if (division === "MOSAIC") {
        if (country === "IN") {
            priceListCode.val("CPL-INR");
        } else {
            priceListCode.val("CPL-USD");
        }
    }

    // TILE
    else if (division === "TILE") {
        if (country === "IN") {
            priceListCode.val("TD-CPL-INR");
        } else {
            priceListCode.val("TD-CPL-USD");
        }
    }
}
function toggleCustomerUnBlock(btn) {

    var customerCode = btn.getAttribute("data-customer");

    if (!confirm("Are you sure you want to unblock this customer?")) {
        return; // ❌ cancel
    }

    $.ajax({
        url: baseURL + 'Customer/CustomerUnblock',
        type: 'POST',
        data: {
            customerCode: customerCode,
            displayRowId: $("#DisplayNo").text(),
            customerName: $("#Name").val(),
            division: $("#Division option:selected").text()
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