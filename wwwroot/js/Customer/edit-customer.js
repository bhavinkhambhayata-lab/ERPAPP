$(document).ready(function () {

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
           class="form-control form-control-sm text-end" value="0"/>
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