$(document).ready(function () {

    $('#GSTGroupCode').change(function () {

        var gstCode = $(this).val();

        $('#HSNSACCode').empty();
        $('#HSNSACCode').append('<option value="">--Select--</option>');

        if (gstCode !== '') {

            $.ajax({
                url: baseURL + 'FGItem/GetFGItemHSNDataWithGSTGroupCode',
                type: 'GET',
                data: { gstGroupCode: gstCode },
                success: function (data) {

                    $.each(data, function (i, item) {
                        $('#HSNSACCode').append(
                            $('<option>', {
                                value: item.code,
                                text: item.name
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

    $("#BaseUnitOfMeasure").change(function () {

        let baseUnit = $(this).val();

        $.ajax({
            url: baseURL + 'FGItem/GetItem_UnitOfMeasureChange',
            type: 'GET',
            data: { baseUnitOfMeasure: baseUnit },
            success: function (response) {

                $("#ProductionBOMNo").empty();
                $("#ProductionBOMNo").append('<option value="">-- Select --</option>');

                $.each(response, function (i, item) {

                    $("#ProductionBOMNo").append(
                        `<option value="${item.code}">${item.name}</option>`
                    );

                });

            }
        });

    });

    $(document).on("click", "#btnPreviewGrades", function () {

        $("#grade-tbbody").html('');

        let selectedGrades = [];

        $(".grade-checkbox:checked").each(function () {

            selectedGrades.push({
                grade: $(this).val(),
                gradeLinkCode: $(this).data("gradelinkcode")
            });

        });

        if (selectedGrades.length == 0) {
            alert("Please select at least one grade.");
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


        // Prefix + Number split
        let prefix = itemCode.match(/[A-Za-z]+/)[0];
        let number = parseInt(itemCode.match(/\d+/)[0]);

        let tbody = "";

        // First Selected Grade
        let firstSelectedGrade = selectedGrades[0];

        // SPRM Item Code
        let sprmItemCode = "";

        // Find SPRM Index
        let sprmIndex = selectedGrades.findIndex(x => x.grade == "SPRM");

        if (sprmIndex != -1) {

            // SPRM Item Code Generate
            if (sprmIndex == 0) {

                sprmItemCode = itemCode;
            }
            else {

                sprmItemCode = prefix + (number + sprmIndex);
            }
        }

        let commonGradeLinkCode = "";

        /*
            SCENARIO 1:
            First Selected = 1ST CHOICE
            AND SPRM selected
    
            => SPRM ItemCode + .1
        */

        if (
            firstSelectedGrade.grade == "1ST CHOICE"
            && sprmIndex != -1
        ) {

            commonGradeLinkCode =
                sprmItemCode + firstSelectedGrade.gradeLinkCode;
        }

        /*
            SCENARIO 2:
            First Selected = SPRM
    
            => ItemCode + .1
        */

        else if (firstSelectedGrade.grade == "SPRM") {

            commonGradeLinkCode =
                itemCode + firstSelectedGrade.gradeLinkCode;
        }

        /*
            SCENARIO 3:
            First Selected = Any Grade
            AND NOT SPRM
    
            => ItemCode + point value
        */

        else {

            commonGradeLinkCode =
                itemCode + firstSelectedGrade.gradeLinkCode;
        }

        for (let i = 0; i < selectedGrades.length; i++) {

            let currentItemCode = "";

            // Item Code Generate
            if (i == 0) {

                currentItemCode = itemCode;
            }
            else {

                currentItemCode = prefix + (number + i);
            }

            tbody += `
                        <tr>

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
                                       value="${commonGradeLinkCode.replace(
                /\.\d+$/,
                selectedGrades[i].gradeLinkCode
            )
                }" readonly />
                            </td>
                             <td>
                                <select class="form-select txt-salesunit" disabled>
                                    <option value="">-- Select --</option>
                                    ${salesUnitOptions}
                                </select>
                            </td>
                        </tr>`;
        }

        $("#grade-tbbody").html(tbody);


        $("#grade-tbbody tr").each(function () {

            let grade = $(this).find(".txt-grade").val();

            // Sales Unit Logic
            if (grade == "SMPL") {

                $(this).find(".txt-salesunit").val("PCS");
            }
            else {

                $(this).find(".txt-salesunit").val("BOX");
            }

            // Brand Default = Griffine
            $(this).find(".txt-brand").val("GRIFINE");

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

        let formData = new FormData();

        // ---------------- General Details ----------------

        formData.append("DisplayNo", $("#DisplayNo").text());

        formData.append("Description", $("#Description").val());
        formData.append("Description2", $("#Description2").val());
        formData.append("RoundingPrecision", $("#RoundingPrecision").val());
        formData.append("GrossWeight", $("#GrossWeight").val());
        formData.append("NetWeight", $("#NetWeight").val());

        formData.append("BaseUnitOfMeasure", $("#BaseUnitOfMeasure").val());

        formData.append("ProductionBOMNo", $("#ProductionBOMNo").val());
        formData.append("RoutingNo", $("#RoutingNo").val());
        formData.append("ManufacturingPolicy", $("#ManufacturingPolicy").val());
        formData.append("ReplenishmentSystem", $("#ReplenishmentSystem").val());
        formData.append("MovementType", $("#MovementType").val());
        formData.append("ReorderingPolicy", $("#ReorderingPolicy").val());
        formData.append("ItemCategoryCode", $("#ItemCategoryCode").val());
        formData.append("PurchUnitOfMeasure", $("#PurchUnitOfMeasure").val());

        // ---------------- Item Specification ----------------

        formData.append("TypeOfProduct", $("#TypeOfProduct").val());
        formData.append("Category", $("#Category").val());
        formData.append("SizeOfTile", $("#SizeOfTile").val());
        formData.append("Brand", $("#Brand").val());
        formData.append("Collection", $("#Collection").val());
        formData.append("SurfaceFinishOrGlaze", $("#SurfaceFinishOrGlaze").val());
        formData.append("GlazeEffect", $("#GlazeEffect").val());
        formData.append("DesignColor", $("#DesignColor").val());
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

        $.ajax({
            url: baseURL + "FGItem/SaveFGItemMaster",
            type: "POST",
            data: formData,

            processData: false,
            contentType: false,

            success: function (response) {

                if (response.success) {

                    showToast(response.message, "success", 4000);

                } else {

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

            $('#ManufacturingPolicy').val('2');
            $('#ReplenishmentSystem').val('1');
            $('#ReorderingPolicy').val('3');

            $('.purch-unit-of-measure').removeClass('d-none');
        }
        else {

            $('#ManufacturingPolicy').val('1');
            $('#ReplenishmentSystem').val('2');
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
});

