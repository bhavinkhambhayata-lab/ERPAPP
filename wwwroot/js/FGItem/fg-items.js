$(document).ready(function () {

    var isCreatedWIPItem = false;

    $(document).on("click", "#btnCheckInERP", function () {

        let isValid = true;

        let fields = [
            { id: "Description", label: "Description", maxLength: 100 },
            { id: "Description2", label: "Description 2", maxLength: 50 },
            { id: "Category", label: "Category" },
            { id: "SizeOfTile", label: "Size Of Tile" },
            { id: "Thickness", label: "Thickness" },
            { id: "Packaging", label: "Packaging" }
        ];

        fields.forEach(function (field) {

            let value = $("#" + field.id).val();

            value = value ? value.trim() : "";

            if (!value) {

                $("span[data-valmsg-for='" + field.id + "']")
                    .text(field.label + " is required.");

                isValid = false;
            }
            else if (field.maxLength && value.length > field.maxLength) {

                $("span[data-valmsg-for='" + field.id + "']")
                    .text(field.label + " cannot exceed " + field.maxLength + " characters.");

                isValid = false;
            }
            else {

                $("span[data-valmsg-for='" + field.id + "']").text("");
            }
        });

        if (!isValid) {
            return;
        }

        let model = {
            description: $("#Description").val().trim(),
            description2: $("#Description2").val().trim(),
            category: $("#Category").val(),
            sizeOfTile: $("#SizeOfTile").val(),
            thickness: $("#Thickness").val(),
            packaging: $("#Packaging").val()
        };

        $.ajax({
            url: baseURL + 'FGItem/CheckItemInERP',
            type: 'POST',
            data: model,
            success: function (response) {

                if (response.success) {

                    if (confirm("This item already exists in ERP. Do you want to add grade in this item?")) {

                        getFGItemEditModel = model;

                        $("#detailsBtn").click();
                    }
                    else {
                        $("#generalDetailsFieldset").prop("disabled", false);
                        $("#itemSpeficationFieldset").prop("disabled", false);
                        $("#gradeFieldset").prop("disabled", false);
                        $("#costandPostingFieldset").prop("disabled", false);
                        $("#btnSaveFGItemMaster").prop("disabled", false);
                    }
                }
                else {

                    if (confirm("This item does not exist in ERP. Do you want to continue adding item details?")) {
                        $("#generalDetailsFieldset").prop("disabled", false);
                        $("#itemSpeficationFieldset").prop("disabled", false);
                        $("#gradeFieldset").prop("disabled", false);
                        $("#costandPostingFieldset").prop("disabled", false);
                        $("#btnSaveFGItemMaster").prop("disabled", false);
                    }
                }
            },
            error: function () {

                toastr.error("Something went wrong.");
            }
        });

    });

   
    $("#Description").on("input", function () {
        $("#DesignColor").val($(this).val());
    });

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

    //$("#BaseUnitOfMeasure").change(function () {

    //    let baseUnit = $(this).val();

    //    $.ajax({
    //        url: baseURL + 'FGItem/GetItem_UnitOfMeasureChange',
    //        type: 'GET',
    //        data: { baseUnitOfMeasure: baseUnit },
    //        success: function (response) {

    //            $("#ProductionBOMNo").empty();
    //            $("#ProductionBOMNo").append('<option value="">-- Select --</option>');

    //            $.each(response, function (i, item) {

    //                $("#ProductionBOMNo").append(
    //                    `<option value="${item.code}">${item.name}</option>`
    //                );

    //            });

    //        }
    //    });

    //});

    $(document).on("click", "#btnPreviewGrades", function () {

        // CHECK GenProdPostingGroup
        if ($("#GenProdPostingGroup").val() == "") {

            showToast("Please select Gen Prod Posting Group.", "danger");
            $("#GenProdPostingGroup").focus();
            return;
        }

        $("#grade-tbbody").html('');

        let selectedGrades = [];

        // ===============================
        // GRADE + COUNT LOGIC
        // ===============================

        $(".grade-checkbox:checked").each(function () {

            let grade = $(this).val();

            let gradeLinkCode =
                $(this).data("gradelinkcode");

            let count = parseInt(
                $(this)
                    .closest(".grade-card")
                    .find(".grade-count")
                    .val()
            ) || 1;

            // Count wise multiple rows
            for (let i = 0; i < count; i++) {

                selectedGrades.push({
                    grade: grade,
                    gradeLinkCode: gradeLinkCode
                });
            }

        });

        if (selectedGrades.length == 0) {

            showToast("Please select at least one grade.", "danger");
            return;
        }

        let itemCode = "";

        $.ajax({
            url: baseURL + 'FGItem/GetFGItemCompanyLastNoUsedCompanyCode',
            type: 'GET',
            async: false,
            success: function (response) {

                itemCode = response;
            }
        });

        let wipItemCode = "";

        $.ajax({
            url: baseURL + 'FGItem/GetWIPItemCompanyLastNoUsedCompanyCode',
            type: 'GET',
            async: false,
            success: function (response) {

                wipItemCode = response;
            }
        });

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

        // ===============================
        // ITEM CODE SPLIT
        // ===============================

        let prefix = itemCode.match(/[A-Za-z]+/)[0];

        let number = parseInt(
            itemCode.match(/\d+/)[0]
        );

        let tbody = "";

        // ===============================
        // SPRM INDEX
        // ===============================

        let sprmIndex = selectedGrades.findIndex(
            x => x.grade === "SPRM"
        );

        // ===============================
        // ROW CREATE
        // ===============================

        let runningNumber = number;

        for (let i = 0; i < selectedGrades.length; i++) {

            let currentItemCode = "";

            // First item
            if (i == 0) {

                currentItemCode = itemCode;
            }
            else {

                runningNumber++;

                currentItemCode =
                    prefix + runningNumber;
            }

            tbody += `
        <tr>

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
                       value="${selectedGrades[i].grade}"
                       disabled />
            </td>

            <td>
                 <input type="text"
           class="form-control txt-gradelinkcode"
           value="${itemCode + selectedGrades[i].gradeLinkCode}"
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

        // ===============================
        // WIP ROW
        // ===============================

        if (sprmIndex != -1 && isCreatedWIPItem == true) {

            tbody += `
        <tr class="wip-row">

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

        $("#grade-tbbody").html(tbody);

        // ===============================
        // SALES UNIT + BRAND LOGIC
        // ===============================

        $("#grade-tbbody tr").each(function () {

            let grade =
                $(this).find(".txt-grade").val();

            let itemCode =
                $(this).find(".txt-itemcode").val();

            let salesUnit =
                $(this).find(".txt-salesunit");

            // WIP Row
            if (
                itemCode.startsWith("WIP")
                && (!grade || grade.trim() == "")
            ) {

                salesUnit.val("BOX");

                salesUnit.prop("disabled", true);
            }
            else {

                // Sales Unit Default
                if (grade == "SMPL") {

                    salesUnit.val("PCS");
                }
                else {

                    salesUnit.val("BOX");
                }

                // Disable Logic
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

            // Brand Default
            $(this).find(".txt-brand")
                .val("GRIFINE");

        });

    });

    $(document).ready(function () {

        $("#btnShowMoreGrades").click(function () {

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

    $("#btnSaveFGItemMaster").on("click", function () {

        if (IsItemsExists()) {

            let confirmContinue = confirm("Some FG items from the selected grade list are already created in ERP. Do you want to continue with the latest FG items?");

            if (!confirmContinue) {
                return;
            }

            // AUTO CLICK PREVIEW BUTTON
            $("#btnPreviewGrades").trigger("click");
        }

        let formData = new FormData();

        // ---------------- General Details ----------------

        formData.append("DisplayNo", $("#DisplayNo").text());

        formData.append("Description", $("#Description").val().trim());
        formData.append("Description2", $("#Description2").val().trim());
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
        formData.append("DesignColor", $("#DesignColor").val().trim() ?? '');
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

        $("#grade-tbbody tr").each(function (index) {

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

        if ($("#grade-tbbody tr").length == 0) {

            showToast("Please select at least one grade.","danger");
            return false;
        }

        // ===============================
        // DUPLICATE GRADE + BRAND VALIDATION
        // ===============================

        let duplicateGradeBrand = {};

        let isValidationFailed = false;

        $("#grade-tbbody tr").each(function () {

            let currentRow = $(this);

            let itemCode = currentRow
                .find(".txt-itemcode")
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

            // Skip WIP Row
            if (
                itemCode &&
                itemCode.startsWith("WIP")
            ) {
                return true;
            }

            // Brand Required
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

            // Create Unique Key
            let uniqueKey =
                `${grade}_${brand}`;

            // Duplicate Check
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

            // Store Key
            duplicateGradeBrand[uniqueKey] = true;

        });

        // Stop Save
        if (isValidationFailed) {
            return;
        }

        showLoader();

        $.ajax({
            url: baseURL + "FGItem/SaveFGItemMaster",
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

    //$('#ItemCategoryCode').on('change', function () {

    //    var selectedOption = $(this).find('option:selected');

    //    if ($(this).val() != '') {

    //        $('#CostingMethod').val(
    //            selectedOption.data('costingmethod')
    //        );

    //        $('#InventoryPostingGroup').val(
    //            selectedOption.data('inventorypostinggroup')
    //        );

    //        $('#GenProdPostingGroup').val(
    //            selectedOption.data('genprodpostinggroup')
    //        );

    //    } else {

    //        $('#CostingMethod').val('');
    //        $('#InventoryPostingGroup').val('');
    //        $('#GenProdPostingGroup').val('');
    //    }
    //});

    $('#GenProdPostingGroup').on('change', function () {

        var val = $(this).val();

        // First clear all values
        $('#ManufacturingPolicy').val('');
        $('#ReplenishmentSystem').val('');
        $('#ReorderingPolicy').val('');

        if (val.includes('-T')) {

            isCreatedWIPItem = false;

            $('#ManufacturingPolicy').val('1');
            $('#ReplenishmentSystem').val('0');
            $('#ReorderingPolicy').val('3');

            $('.purch-unit-of-measure').removeClass('d-none');
        }
        else {

            isCreatedWIPItem = true;

            $('#ManufacturingPolicy').val('0');
            $('#ReplenishmentSystem').val('1');
            $('#ReorderingPolicy').val('3');

            $('.purch-unit-of-measure').addClass('d-none');

        }

    });



    $("#BaseUnitOfMeasure").val('SQM').trigger('change');
    $("#SalesUnitOfMeasureWithSMPLGrade").val('PCS').trigger('change');
    $("#SalesUnitOfMeasureWithOtherGrade").val('BOX').trigger('change');
    $("#CostingMethod").val('0').trigger('change');
    $("#GSTGroupCode").val('18_GOODS').trigger('change');
    $("#RoundingPrecision").val('0.001');
    $("#GSTCredit").val("1");

    $("#generalDetailsFieldset").prop("disabled", true);
    $("#itemSpeficationFieldset").prop("disabled", true);
    $("#gradeFieldset").prop("disabled", true);
    $("#costandPostingFieldset").prop("disabled", true);
    $("#btnSaveFGItemMaster").prop("disabled", true);
});

function IsItemsExists() {

    let itemNos = [];

    $("#grade-tbbody tr").each(function () {

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