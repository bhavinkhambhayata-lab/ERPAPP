$(function () {
    $("#editFixedAssetForm :input").prop("disabled", true);
    $("#btnBackFixedAssetListPage").prop("disabled", false);
    $("#btnFixedAssetUnBlockToggle").prop("disabled", false);
})

function backFixedAssetListPage() {
    editFixedAssetNo = '';
    $('#listBtn').click();
}

function toggleFixedAssetUnBlock(btn) {

    var fixedAssetCode = btn.getAttribute("data-fixed-asset");

    if (!confirm("Are you sure you want to unblock this fixed asset?")) {
        return; // ❌ cancel
    }

    $.ajax({
        url: '/FixedAsset/FixedAssetUnblock',
        type: 'POST',
        data: {
            fixedAssetCode: fixedAssetCode
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