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

    // COUNTRY CHANGE
    $('#CountryCode').change(function () {

        var countryCode = $(this).val();

        // 🔥 FULL RESET
        $('#StateCode').empty().append('<option value="">Select State</option>');
        $('#CityCode').empty().append('<option value="">Select City</option>');
        $('#PostCode').empty().append('<option value="">Select Post Code</option>');
        $('#Region').val('');
        $('#Zone').val('');

        if (countryCode === "") return;

        $.ajax({
            url: '/Customer/GetCustomerAddress',
            type: 'GET',
            data: {
                type: 'STATE',
                countryCode: countryCode
            },
            success: function (response) {

                if (!response || response.length === 0) return;

                if (response[0].dataType === "STATE") {

                    $.each(response, function (i, item) {
                        $('#StateCode').append(
                            $('<option>', {
                                value: item.code,
                                text: item.name
                            })
                        );
                    });

                } else {

                    $('#StateCode').prop('disabled', true);

                    // Fallback → Direct City
                    $.each(response, function (i, item) {
                        $('#CityCode').append(
                            $('<option>', {
                                value: item.code,
                                text: item.name
                            }).attr('data-state', item.state)
                        );
                    });
                }
            }
        });
    });


    // STATE CHANGE
    $('#StateCode').change(function () {

        var stateCode = $(this).val();
        var countryCode = $('#CountryCode').val();

        // 🔥 RESET BELOW LEVELS
        $('#CityCode').empty().append('<option value="">Select City</option>');
        $('#PostCode').empty().append('<option value="">Select Post Code</option>');
        $('#Region').val('');
        $('#Zone').val('');

        if (stateCode === "") return;

        $.ajax({
            url: '/Customer/GetCustomerAddress',
            type: 'GET',
            data: {
                type: 'CITY',
                countryCode: countryCode,
                state: stateCode
            },
            success: function (response) {

                if (!response) return;

                $.each(response, function (i, item) {
                    $('#CityCode').append(
                        $('<option>', {
                            value: item.code,
                            text: item.name
                        })
                    );
                });

            }
        });
    });


    // CITY CHANGE
    $('#CityCode').change(function () {

        var city = $(this).val();

        // 🔥 RESET BELOW LEVELS
        $('#PostCode').empty().append('<option value="">Select Post Code</option>');
        $('#Region').val('');
        $('#Zone').val('');

        if (city === "") return;

        $.ajax({
            url: '/Customer/GetCustomerAddress',
            type: 'GET',
            data: {
                type: 'CODE',
                city: city
            },
            success: function (response) {

                if (!response) return;

                $.each(response, function (i, item) {
                    $('#PostCode').append(
                        $('<option>', {
                            value: item.code,
                            text: item.name
                        })
                    );
                });

            }
        });
    });


    // POST CODE CHANGE
    $('#PostCode').change(function () {

        var postCode = $(this).val();

        // 🔥 RESET DETAIL FIELDS
        $('#Region').val('');
        $('#Zone').val('');

        if (postCode === "") return;

        $.ajax({
            url: '/Customer/GetPostCodeDetail',
            type: 'GET',
            data: { postCode: postCode },
            success: function (response) {

                if (response) {
                    $('#Region').val(response.region || '');
                    $('#Zone').val(response.zone || '');
                }
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