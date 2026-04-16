$(document).ready(function () {
});

function backVendorListPage() {
    editVendorNo = '';
    editVendorBlock = '0';
    $('#listBtn').click();
}

function toggleVendorUnBlock(btn) {

    var vendorCode = btn.getAttribute("data-vendor");

    if (!confirm("Are you sure you want to unblock this vendor?")) {
        return; // ❌ cancel
    }

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

                showToast(res.message, "success", 3000);

                btn.style.display = "none";

            } else {
                alert(res.message || "Something went wrong!");
            }
        }
    });
}