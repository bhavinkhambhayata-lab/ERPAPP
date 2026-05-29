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

        // =====================================
        // REMOVE OLD PREVIEW ROWS
        // =====================================

        $("#grade-tbbody .preview-row").remove();
        $("#grade-tbbody .wip-row").remove();

        // =====================================
        // EXISTING ROWS
        // =====================================

        let existingRows = [];

        $("#grade-tbbody .existing-row").each(function () {

            existingRows.push({
                itemCode: $(this).find(".txt-gradeitemcode").val(),
                grade: $(this).find(".txt-grade").val(),
                gradeLinkCode: $(this).find(".txt-gradelinkcode").val()
            });

        });

        // =====================================
        // NEW ROWS TO CREATE
        // =====================================

        let rowsToGenerate = [];

        // =====================================
        // 1. NEW CHECKED GRADES
        // =====================================

        $(".grade-checkbox")
            .not(":disabled")
            .filter(":checked")
            .each(function () {

                rowsToGenerate.push({
                    grade: $(this).val(),
                    gradeLinkCode: $(this).data("gradelinkcode"),
                    isExistingGradeExtraCount: false
                });

            });

        // =====================================
        // 2. EXISTING GRADE COUNT INCREASE
        // =====================================

        $(".grade-checkbox:disabled").each(function () {

            let checkbox = $(this);

            let grade = checkbox.val();

            let countTextbox =
                checkbox.closest(".grade-card").find(".grade-count");

            let oldCount =
                parseInt(countTextbox.attr("data-oldcount")) || 1;

            let newCount =
                parseInt(countTextbox.val()) || 1;

            // ONLY ADDITIONAL
            if (newCount > oldCount) {

                let extraCount = newCount - oldCount;

                for (let i = 0; i < extraCount; i++) {

                    rowsToGenerate.push({
                        grade: grade,
                        gradeLinkCode: checkbox.data("gradelinkcode"),
                        isExistingGradeExtraCount: true
                    });

                }
            }

        });

        // =====================================
        // NOTHING TO GENERATE
        // =====================================

        if (rowsToGenerate.length == 0) {

            alert("No new grades/count changes found.");
            return;
        }

        // =====================================
        // EXISTING SPRM CHECK
        // =====================================

        let existingSPRM =
            existingRows.some(x => x.grade == "SPRM");

        // =====================================
        // NEW SPRM CHECK
        // =====================================

        let newSPRM =
            rowsToGenerate.some(x => x.grade == "SPRM");

        // =====================================
        // GET FG ITEM CODE
        // =====================================

        let itemCode = "";

        $.ajax({
            url: baseURL + 'FGItem/GetFGItemCompanyLastNoUsedCompanyCode',
            type: 'GET',
            async: false,
            success: function (response) {

                itemCode = response;
            }
        });

        // =====================================
        // GET WIP ITEM CODE
        // =====================================

        let wipItemCode = "";

        $.ajax({
            url: baseURL + 'FGItem/GetWIPItemCompanyLastNoUsedCompanyCode',
            type: 'GET',
            async: false,
            success: function (response) {

                wipItemCode = response;
            }
        });

        // =====================================
        // SALES UNIT OPTIONS
        // =====================================

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

        // =====================================
        // BRAND OPTIONS
        // =====================================

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

        // =====================================
        // ITEM CODE SPLIT
        // =====================================

        let prefix = itemCode.match(/[A-Za-z]+/)[0];
        let number = parseInt(itemCode.match(/\d+/)[0]);

        // =====================================
        // WIP CHECK
        // =====================================

        var isCreatedWIPItem = false;

        let val = $('#GenProdPostingGroup').val();

        isCreatedWIPItem = !val.includes('-T');

        // =====================================
        // TBODY
        // =====================================

        let tbody = "";

        // =====================================
        // EXISTING BASE LINK CODE
        // =====================================

        let baseLinkCode = "";

        if (existingSPRM) {

            let sprmBase =
                existingRows.find(x => x.grade == "SPRM");

            baseLinkCode =
                sprmBase.gradeLinkCode.replace(/\.\d+$/, '');
        }
        else {

            if (existingRows.length > 0) {

                baseLinkCode =
                    existingRows[0].gradeLinkCode.replace(/\.\d+$/, '');
            }
        }

        // =====================================
        // IF NEW SPRM
        // =====================================

        if (!existingSPRM && newSPRM) {

            let sprmIndex =
                rowsToGenerate.findIndex(x => x.grade == "SPRM");

            let sprmItemCode =
                prefix + (number + sprmIndex);

            baseLinkCode = sprmItemCode;

            // UPDATE EXISTING ROWS
            $("#grade-tbbody .existing-row").each(function () {

                let existingGradeLinkCode =
                    $(this).find(".txt-gradelinkcode").val();

                let suffix =
                    existingGradeLinkCode.match(/\.\d+$/);

                suffix = suffix ? suffix[0] : "";

                $(this)
                    .find(".txt-gradelinkcode")
                    .val(baseLinkCode + suffix);

            });
        }

        // =====================================
        // GENERATE ROWS
        // =====================================

        for (let i = 0; i < rowsToGenerate.length; i++) {

            let currentItemCode =
                prefix + (number + i);

            let currentGrade =
                rowsToGenerate[i].grade;

            // SAME GRADE LINK CODE LOGIC
            let currentGradeLinkCode =
                baseLinkCode + rowsToGenerate[i].gradeLinkCode;

            tbody += `
        <tr class="preview-row">

            <td>
                <input type="text"
                       class="form-control txt-itemcode"
                       value="${currentItemCode}"
                       disabled />
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
                       value="${currentGrade}"
                       disabled />
            </td>

            <td>
                <input type="text"
                       class="form-control txt-gradelinkcode"
                       value="${currentGradeLinkCode}"
                       disabled />
            </td>

            <td>
                <select class="form-select txt-salesunit">
                    <option value="">-- Select --</option>
                    ${salesUnitOptions}
                </select>
            </td>

        </tr>`;

        }

        // =====================================
        // WIP ROW
        // =====================================

        if (!existingSPRM && newSPRM && isCreatedWIPItem) {

            tbody += `
        <tr class="wip-row preview-row">

            <td>
                <input type="text"
                       class="form-control txt-itemcode"
                       value="${wipItemCode}"
                       disabled />
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
                       disabled />
            </td>

            <td>
                <input type="text"
                       class="form-control txt-gradelinkcode"
                       value=""
                       disabled />
            </td>

            <td>
                <select class="form-select txt-salesunit">
                    <option value="">-- Select --</option>
                    ${salesUnitOptions}
                </select>
            </td>

        </tr>`;
        }

        // =====================================
        // APPEND
        // =====================================

        $("#grade-tbbody").append(tbody);

        // =====================================
        // DEFAULT VALUES
        // =====================================

        $("#grade-tbbody .preview-row").each(function () {

            let grade =
                $(this).find(".txt-grade").val();

            let itemCode =
                $(this).find(".txt-itemcode").val();

            let salesUnit =
                $(this).find(".txt-salesunit");

            // =================================
            // WIP
            // =================================

            if (itemCode.startsWith("WIP") && (!grade || grade.trim() == "")) {

                salesUnit.val("BOX");
                salesUnit.prop("disabled", true);
            }
            else {

                // =============================
                // SALES UNIT
                // =============================

                if (grade == "SMPL") {

                    salesUnit.val("PCS");
                }
                else {

                    salesUnit.val("BOX");
                }

                // =============================
                // DISABLE
                // =============================

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
            }

            // =================================
            // BRAND
            // =================================

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
        //formData.append("ColourFamily", $("#ColourFamily").val());
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

        if ($("#grade-tbbody .preview-row").length == 0) {

            showToast("Please select at least one grade.", "danger");
            return false;
        }

        // ===============================
        // DUPLICATE GRADE + BRAND VALIDATION
        // EDIT MODE
        // ===============================

        let duplicateGradeBrand = {};

        let isValidationFailed = false;

        $("#grade-tbbody tr").each(function () {

            let currentRow = $(this);

            let itemCode = currentRow
                .find(".txt-itemcode, .txt-gradeitemcode")
                .val();

            let grade = currentRow
                .find(".txt-grade")
                .val()
                ?.trim();

            let brand = currentRow
                .find(".txt-brand")
                .val()
                ?.trim();

            // Remove old error class
            currentRow
                .find(".txt-brand")
                .removeClass("brand-error-field");

            // ===========================
            // SKIP WIP ROW
            // ===========================

            if (
                itemCode &&
                itemCode.startsWith("WIP")
            ) {
                return true;
            }

            // ===========================
            // BRAND REQUIRED
            // ===========================

            if (!brand) {

                showToast(
                    `Please select brand for Grade "${grade}".`,
                    "danger"
                );

                currentRow
                    .find(".txt-brand")
                    .addClass("brand-error-field")
                    .focus();

                isValidationFailed = true;

                return false;
            }

            // ===========================
            // UNIQUE KEY
            // ===========================

            let uniqueKey = `${grade}_${brand}`;

            // ===========================
            // DUPLICATE CHECK
            // ===========================

            if (duplicateGradeBrand[uniqueKey]) {

                showToast(
                    `Duplicate Brand "${brand}" is not allowed for Grade "${grade}".`,
                    "danger"
                );

                currentRow
                    .find(".txt-brand")
                    .addClass("brand-error-field")
                    .focus();

                isValidationFailed = true;

                return false;
            }

            // ===========================
            // STORE KEY
            // ===========================

            duplicateGradeBrand[uniqueKey] = true;

        });

        // ===============================
        // STOP SAVE
        // ===============================

        if (isValidationFailed) {
            return;
        }


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
                    getFGItemEditModel = '';
                    $("#detailsBtn").click();

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
