$(document).ready(function () {
   
    $('#Category').change(function () {
        $("#GSTGroupCode").val("18_GOODS").trigger("change");
    });

    $('#GSTGroupCode').change(function () {

        var gstCode = $(this).val();

        $('#HSNSACCode').empty();
        $('#HSNSACCode').append('<option value="">--Select--</option>');

        var category = $("#Category").val() ?? '';

        if (gstCode !== '') {

            $.ajax({
                url: baseURL + 'FGItem/GetFGItemHSNDataWithGSTGroupCode',
                type: 'GET',
                data: { gstGroupCode: gstCode, category: category },
                success: function (data) {
                    
                    $.each(data, function (i, item) {
                        $('#HSNSACCode').append(
                            $('<option>', {
                                value: item.code,
                                text: item.name,
                                selected: item.isSelected == true
                            })
                        );
                    });

                },
                error: function () {
                    alert('Error loading HSN/SAC data');
                }
            });

        }
    });

    var category = $("#Category").val();
    $("#Category").val(category).trigger("change");

    $(document).on("click", "#btnEditPreviewGrades", function () {

        // REMOVE ONLY PREVIEW ROWS
        $("#grade-tbbody .preview-row").remove();
        $("#grade-tbbody .wip-row").remove();

        // ============================
        // EXISTING DATA
        // ============================

        let existingRows = [];

        $("#grade-tbbody .existing-row").each(function () {

            existingRows.push({
                itemCode: $(this).find(".txt-gradeitemcode").val(),
                grade: $(this).find(".txt-grade").val(),
                gradeLinkCode: $(this).find(".txt-gradelinkcode").val()
            });

        });

        // ============================
        // NEW SELECTED GRADES
        // ONLY NON-DISABLED
        // ============================

        let selectedGrades = [];

        $(".grade-checkbox")
            .not(":disabled")
            .filter(":checked")
            .each(function () {

                selectedGrades.push({
                    grade: $(this).val(),
                    gradeLinkCode: $(this).data("gradelinkcode")
                });

            });

        if (selectedGrades.length == 0) {

            alert("Please select at least one new grade.");
            return;
        }

        // ============================
        // EXISTING SPRM CHECK
        // ============================

        let existingSPRM = existingRows.some(x => x.grade == "SPRM");

        // ============================
        // NEW SPRM CHECK
        // ============================

        let newSPRM = selectedGrades.some(x => x.grade == "SPRM");

        // ============================
        // GET LAST FG CODE
        // ============================

        let itemCode = "";

        $.ajax({
            url: baseURL + 'FGItem/GetFGItemCompanyLastNoUsedCompanyCode',
            type: 'GET',
            async: false,
            success: function (response) {

                itemCode = response;
            }
        });

        // ============================
        // GET WIP CODE
        // ============================

        let wipItemCode = "";

        $.ajax({
            url: baseURL + 'FGItem/GetWIPItemCompanyLastNoUsedCompanyCode',
            type: 'GET',
            async: false,
            success: function (response) {

                wipItemCode = response;
            }
        });

        // ============================
        // SALES UNIT DROPDOWN
        // ============================

        let salesUnitOptions = "";

        $.ajax({
            url: baseURL + 'FGItem/GetItem_UnitOfMeasureDropDownData',
            type: 'GET',
            async: false,
            success: function (response) {

                $.each(response, function (index, item) {

                    salesUnitOptions += `
                <option value="${item.code}">
                    ${item.name}
                </option>`;
                });
            }
        });

        // ============================
        // BRAND DROPDOWN
        // ============================

        let brandOptions = "";

        $.ajax({
            url: baseURL + 'FGItem/GetItem_BrandDropDownData',
            type: 'GET',
            async: false,
            success: function (response) {

                $.each(response, function (index, item) {

                    brandOptions += `
                <option value="${item.code}">
                    ${item.name}
                </option>`;
                });
            }
        });

        // ============================
        // ITEM CODE SPLIT
        // ============================

        let prefix = itemCode.match(/[A-Za-z]+/)[0];
        let number = parseInt(itemCode.match(/\d+/)[0]);

        var isCreatedWIPItem = false;

        let val = $('#GenProdPostingGroup').val();

        isCreatedWIPItem = !val.includes('-T');


        let tbody = "";

        // ============================================
        // CASE :
        // EXISTING SPRM ALREADY PRESENT
        // ============================================

        if (existingSPRM) {

            // EXISTING SPRM LINK BASE
            let sprmBase = existingRows.find(x => x.grade == "SPRM");

            let baseCode =
                sprmBase.gradeLinkCode.replace(/\.\d+$/, '');

            let startIndex = existingRows.length + 1;

            // ADD ONLY NEW ROWS
            for (let i = 0; i < selectedGrades.length; i++) {

                let currentItemCode =
                    prefix + (number + i);

                tbody += `
            <tr class="preview-row">

                <td>
                    <input type="text"
                           class="form-control txt-itemcode"
                           value="${currentItemCode}"
                           readonly />
                </td>

                <td>
                    <select class="form-select txt-brand">
                        <option value="">-- Select --</option>
                        ${brandOptions}
                    </select>
                </td>

                <td>
                    <input type="text"
                           class="form-control txt-grade"
                           value="${selectedGrades[i].grade}"
                           readonly />
                </td>

                <td>
                    <input type="text"
                           class="form-control txt-gradelinkcode"
                           value="${baseCode}.${startIndex + i}"
                           readonly />
                </td>

                <td>
                    <select class="form-select txt-salesunit">
                        <option value="">-- Select --</option>
                        ${salesUnitOptions}
                    </select>
                </td>

            </tr>`;
            }

        }

        // ============================================
        // CASE :
        // NO EXISTING SPRM
        // BUT NEW SPRM SELECTED
        // ============================================

        else if (newSPRM) {

            let sprmIndex =
                selectedGrades.findIndex(x => x.grade == "SPRM");

            let sprmItemCode =
                prefix + (number + sprmIndex);

            // ====================================
            // UPDATE EXISTING ROWS LINKCODE
            // ====================================

            $("#grade-tbbody .existing-row").each(function (index) {

                $(this)
                    .find(".txt-gradelinkcode")
                    .val(sprmItemCode + "." + (index + 1));

            });

            // ====================================
            // NEW ROWS
            // ====================================

            let startIndex = existingRows.length + 1;

            for (let i = 0; i < selectedGrades.length; i++) {

                let currentItemCode =
                    prefix + (number + i);

                tbody += `
            <tr class="preview-row">

                <td>
                    <input type="text"
                           class="form-control txt-itemcode"
                           value="${currentItemCode}"
                           readonly />
                </td>

                <td>
                    <select class="form-select txt-brand">
                        <option value="">-- Select --</option>
                        ${brandOptions}
                    </select>
                </td>

                <td>
                    <input type="text"
                           class="form-control txt-grade"
                           value="${selectedGrades[i].grade}"
                           readonly />
                </td>

                <td>
                    <input type="text"
                           class="form-control txt-gradelinkcode"
                           value="${sprmItemCode}.${startIndex + i}"
                           readonly />
                </td>

                <td>
                    <select class="form-select txt-salesunit">
                        <option value="">-- Select --</option>
                        ${salesUnitOptions}
                    </select>
                </td>

            </tr>`;
            }

            if (isCreatedWIPItem) {
                // ====================================
                // WIP ROW
                // ====================================

                tbody += `
        <tr class="wip-row preview-row">

            <td>
                <input type="text"
                       class="form-control txt-itemcode"
                       value="${wipItemCode}"
                       readonly />
            </td>

            <td>
                <select class="form-select txt-brand">
                    <option value="">-- Select --</option>
                    ${brandOptions}
                </select>
            </td>

            <td>
                <input type="text"
                       class="form-control txt-grade"
                       value=""
                       readonly />
            </td>

            <td>
                <input type="text"
                       class="form-control txt-gradelinkcode"
                       value=""
                       readonly />
            </td>

            <td>
                <select class="form-select txt-salesunit">
                    <option value="">-- Select --</option>
                    ${salesUnitOptions}
                </select>
            </td>

        </tr>`;
            }
        }

        // ============================================
        // CASE :
        // NORMAL NEW GRADES
        // ============================================

        else {

            let existingLastLinkCode =
                existingRows[0].gradeLinkCode.replace(/\.\d+$/, '');

            let startIndex = existingRows.length + 1;

            for (let i = 0; i < selectedGrades.length; i++) {

                let currentItemCode =
                    prefix + (number + i);

                tbody += `
            <tr class="preview-row">

                <td>
                    <input type="text"
                           class="form-control txt-itemcode"
                           value="${currentItemCode}"
                           readonly />
                </td>

                <td>
                    <select class="form-select txt-brand">
                        <option value="">-- Select --</option>
                        ${brandOptions}
                    </select>
                </td>

                <td>
                    <input type="text"
                           class="form-control txt-grade"
                           value="${selectedGrades[i].grade}"
                           readonly />
                </td>

                <td>
                    <input type="text"
                           class="form-control txt-gradelinkcode"
                           value="${existingLastLinkCode}.${startIndex + i}"
                           readonly />
                </td>

                <td>
                    <select class="form-select txt-salesunit">
                        <option value="">-- Select --</option>
                        ${salesUnitOptions}
                    </select>
                </td>

            </tr>`;
            }
        }

        // ============================
        // APPEND NEW ROWS
        // ============================

        $("#grade-tbbody").append(tbody);

        // ============================
        // DEFAULT VALUES
        // ============================

        $("#grade-tbbody .preview-row").each(function () {

            let grade = $(this).find(".txt-grade").val();
            let salesUnit = $(this).find(".txt-salesunit");

            // SALES UNIT
            if (grade == "SMPL") {

                salesUnit.val("PCS");
            }
            else {

                salesUnit.val("BOX");
            }

            // DISABLE SALES UNIT
            if (
                grade == "SPRM"
                || grade == "STD"
                || grade == "ECO"
                || grade == "SMPL"
            ) {

                salesUnit.prop("disabled", true);
            }
            else {

                salesUnit.prop("disabled", false);
            }

            // DEFAULT BRAND
            $(this).find(".txt-brand").val("GRIFINE");

        });

    });

    $(document).ready(function () {

        $("#btnEditShowMoreGrades").click(function () {

            if ($(this).text().trim() == "More Grade") {

                $(".extra-grade").removeClass("d-none");

                $(this).text("Less Grade");

            }
            else {

                $(".extra-grade").addClass("d-none");

                $(this).text("More Grade");

            }

        });

    });

    $("#btnUpdateFGItemMaster").on("click", function () {

        if (IsItemsEditExists()) {

            let confirmContinue = confirm("Some FG items from the selected grade list are already created in ERP. Do you want to continue with the latest FG items?");

            if (!confirmContinue) {
                return;
            }

            // AUTO CLICK PREVIEW BUTTON
            $("#btnEditPreviewGrades").trigger("click");
        }

        let formData = new FormData();

        // ---------------- General Details ----------------

        formData.append("DisplayNo", $("#DisplayNo").text());

        formData.append("Description", $("#Description").val());
        formData.append("Description2", $("#Description2").val());
        formData.append("RoundingPrecision", $("#RoundingPrecision").val());
        formData.append("GrossWeight", $("#GrossWeight").val());
        formData.append("NetWeight", $("#NetWeight").val());

        formData.append("BaseUnitOfMeasure", $("#BaseUnitOfMeasure").val());

        //formData.append("ProductionBOMNo", $("#ProductionBOMNo").val() ?? '');
        //formData.append("RoutingNo", $("#RoutingNo").val() ?? '');
        formData.append("ManufacturingPolicy", $("#ManufacturingPolicy").val());
        formData.append("ReplenishmentSystem", $("#ReplenishmentSystem").val());
        formData.append("MovementType", $("#MovementType").val());
        formData.append("ReorderingPolicy", $("#ReorderingPolicy").val());
        //formData.append("ItemCategoryCode", $("#ItemCategoryCode").val() ?? '');
        formData.append("PurchUnitOfMeasure", $("#PurchUnitOfMeasure").val());

        // ---------------- Item Specification ----------------

        formData.append("TypeOfProduct", $("#TypeOfProduct").val());
        formData.append("Category", $("#Category").val());
        formData.append("SizeOfTile", $("#SizeOfTile").val());
     
        formData.append("Collection", $("#Collection").val());
        formData.append("SurfaceFinishOrGlaze", $("#SurfaceFinishOrGlaze").val());
        formData.append("GlazeEffect", $("#GlazeEffect").val());
        formData.append("DesignColor", $("#DesignColor").val() ?? '');
        formData.append("ColourFamily", $("#ColourFamily").val());
        formData.append("TypeOfTile", $("#TypeOfTile").val());
        formData.append("Packaging", $("#Packaging").val());
        formData.append("Thickness", $("#Thickness").val());
        formData.append("Body", $("#Body").val());
        formData.append("PLCollection", $("#PLCollection").val());
        formData.append("PLColours", $("#PLColours").val());

        // ---------------- Cost & Posting ----------------

        formData.append("CostingMethod", $("#CostingMethod").val());
        formData.append("GenProdPostingGroup", $("#GenProdPostingGroup").val());
        formData.append("GSTGroupCode", $("#GSTGroupCode").val());
        formData.append("HSNSACCode", $("#HSNSACCode").val());
        formData.append("GSTCredit", $("#GSTCredit").val());
        formData.append("InventoryPostingGroup", $("#InventoryPostingGroup").val());

        // ---------------- Grade List ----------------

        $("#grade-tbbody .preview-row").each(function (index) {

            formData.append(`GradeListDetails[${index}].GradeItemCode`,
                $(this).find(".txt-itemcode").val());

            formData.append(`GradeListDetails[${index}].Brand`,
                $(this).find(".txt-brand").val());

            formData.append(`GradeListDetails[${index}].Grade`,
                $(this).find(".txt-grade").val());

            formData.append(`GradeListDetails[${index}].GradeLinkCode`,
                $(this).find(".txt-gradelinkcode").val());

            formData.append(`GradeListDetails[${index}].SalesUnitOfMeasure`,
                $(this).find(".txt-salesunit").val());

        });

        showLoader();

        $.ajax({
            url: baseURL + "FGItem/UpdateFGItemMaster",
            type: "POST",
            data: formData,

            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {
                    hideLoader();
                    showToast(response.message, "success", 4000);

                } else {

                    hideLoader();
                    $('.text-danger').text('');

                    if (response.errors) {

                        $.each(response.errors, function (key, messages) {

                            $('[data-valmsg-for="' + key + '"]')
                                .text(messages[0]);

                        });
                    }
                }
            },

            error: function (xhr) {
                hideLoader();
                console.log(xhr);
                toastr.error("Something went wrong.");

            }
        });

    });

    
});
function IsItemsEditExists() {

    let itemNos = [];

    $("#grade-tbbody .preview-row").each(function () {

        let itemCode = $(this).find(".txt-itemcode").val();

        if (itemCode) {
            itemNos.push(itemCode);
        }
    });

    let isExists = 0;

    $.ajax({
        url: baseURL + "FGItem/CheckMultipleItemsExists",
        type: "GET",
        data: { itemNos: itemNos.join(",") },
        async: false,
        success: function (response) {

            // Item exists in ERP
            if (response.success) {

                isExists = 1;
            }
        }
    });

    return isExists;
}
