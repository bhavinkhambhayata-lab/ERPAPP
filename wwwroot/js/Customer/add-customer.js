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

                $("#City").val(data.city);
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
    $("#CityName").on("input", function () {

        if ($(this).val().trim() === "") {

            $("#CountryCode").val('');
            $("#StateCode").val('');
            $("#PostCode").empty().append('<option value="">--Select--</option>');
            $("#Region").val('');
            $("#Zone").val('');

        }

    });

    // CITY AUTOCOMPLETE
    $("#CityName").autocomplete({

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

        minLength: 2,

        select: function (event, ui) {

            $("#CityName").val(ui.item.value);

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

                    if (data) {
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

        if (!form.valid()) {
            return;
        }

        $.ajax({
            url: '/Customer/SaveCustomerMaster',
            type: 'POST',
            data: form.serialize(),
            success: function (response) {

                if (response.success) {

                    alert(response.message);

                    // Optional reset form
                    form[0].reset();

                    // Or redirect
                    // window.location.href = '/Customer/CustomerList';
                }
                else {

                    if (response.errors) {
                        showServerErrors(response.errors);
                    }
                    else {
                        alert(response.message);
                    }
                }
            },
            error: function () {
                alert("Server error occurred.");
            }
        });
    });

});



function addBrandRow() {

    var table = document.getElementById("brandTable")
        .getElementsByTagName('tbody')[0];

    var rowCount = table.rows.length;
    var row = table.insertRow(rowCount);

    row.innerHTML = `
            <td><input name="CustomerList[${rowCount}].CustomerNo" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].BrandCode" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].CustomerCategoryCode" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].TradeSecurityAmount" type="number" class="form-control form-control-sm text-end" /></td>
            <td><input name="CustomerList[${rowCount}].CustomerDiscountGroup" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].DealerClassification" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].Allocation" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].SalesPersonCode" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].HOSalesPerson" class="form-control form-control-sm" /></td>
             <td><input name="CustomerList[${rowCount}].DLRAppointmentDate" class="form-control form-control-sm" /></td>
            <td><input name="CustomerList[${rowCount}].DLRTerminationDate" class="form-control form-control-sm" /></td>
            <td class="text-center">
                <button type="button" class="btn btn-danger btn-sm" onclick="removeRow(this)">
                    ✕
                </button>
            </td>
        `;
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