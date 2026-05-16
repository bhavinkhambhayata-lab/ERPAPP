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

                        </tr>`;
        }

        $("#grade-tbbody").html(tbody);

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

        // ---------------------------------------------
        // Grade List Data
        // ---------------------------------------------
        let gradeList = [];

        $("#grade-tbbody tr").each(function () {

            gradeList.push({
                GradeItemCode: $(this).find(".txt-itemcode").val(),
                Grade: $(this).find(".txt-grade").val(),
                GradeLinkCode: $(this).find(".txt-gradelinkcode").val()
            });

        });

        console.log(gradeList);

        // ---------------------------------------------
        // Main Model Data
        // ---------------------------------------------
        let model = {

            DisplayNo: $("#DisplayNo").text(),

            // ---------------- General Details ----------------
            Description: $("#Description").val(),
            Description2: $("#Description2").val(),
            RoundingPrecision: $("#RoundingPrecision").val(),
            GrossWeight: $("#GrossWeight").val(),
            NetWeight: $("#NetWeight").val(),

            BaseUnitOfMeasure: $("#BaseUnitOfMeasure").val(),

            ProductionBOMNo: $("#ProductionBOMNo").val(),
            RoutingNo: $("#RoutingNo").val(),
            ManufacturingPolicy: $("#ManufacturingPolicy").val(),
            ReplenishmentSystem: $("#ReplenishmentSystem").val(),
            MovementType: $("#MovementType").val(),
            ReorderingPolicy: $("#ReorderingPolicy").val(),
            ItemCategoryCode: $("#ItemCategoryCode").val(),
            SalesUnitOfMeasureWithOtherGrade: $("#SalesUnitOfMeasureWithOtherGrade").val(),
            SalesUnitOfMeasureWithSMPLGrade: $("#SalesUnitOfMeasureWithSMPLGrade").val(),
            PurchUnitOfMeasure: $("#PurchUnitOfMeasure").val(),

            // ---------------- Item Specification ----------------
            TypeOfProduct: $("#TypeOfProduct").val(),
            Category: $("#Category").val(),
            SizeOfTile: $("#SizeOfTile").val(),
            Brand: $("#Brand").val(),
            Collection: $("#Collection").val(),
            SurfaceFinishOrGlaze: $("#SurfaceFinishOrGlaze").val(),
            GlazeEffect: $("#GlazeEffect").val(),
            DesignColor: $("#DesignColor").val(),
            ColourFamily: $("#ColourFamily").val(),
            TypeOfTile: $("#TypeOfTile").val(),
            PackagingWithOtherGrade: $("#PackagingWithOtherGrade").val(),
            PackagingWithSMPLGrade: $("#PackagingWithSMPLGrade").val(),
            Thickness: $("#Thickness").val(),
            Body: $("#Body").val(),
            PLCollection: $("#PLCollection").val(),
            PLColours: $("#PLColours").val(),

            // ---------------- Cost & Posting ----------------
            CostingMethod: $("#CostingMethod").val(),
            GenProdPostingGroup: $("#GenProdPostingGroup").val(),
            GSTGroupCode: $("#GSTGroupCode").val(),
            HSNSACCode: $("#HSNSACCode").val(),
            GSTCredit: $("#GSTCredit").val(),
            InventoryPostingGroup: $("#InventoryPostingGroup").val(),

            // ---------------- Grade List ----------------
            GradeListDetails: gradeList
        };

        console.log(model);

        // ---------------------------------------------
        // AJAX CALL
        // ---------------------------------------------
        $.ajax({
            url: baseURL + "FGItem/SaveFGItemMaster",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(model),

            success: function (response) {

                if (response.success) {

                    showToast(response.message, "success", 4000);
                } else {

                    showToast(response.message, "danger", 4000);
                }

            },

            error: function (xhr) {

                toastr.error("Something went wrong.");
                console.log(xhr);

            }
        });

    });

    $('#ItemCategoryCode').on('change', function () {

        var selectedOption = $(this).find('option:selected');

        if ($(this).val() != '') {

            $('#CostingMethod').val(
                selectedOption.data('costingmethod')
            );

            $('#InventoryPostingGroup').val(
                selectedOption.data('inventorypostinggroup')
            );

            $('#GenProdPostingGroup').val(
                selectedOption.data('genprodpostinggroup')
            );

        } else {

            $('#CostingMethod').val('');
            $('#InventoryPostingGroup').val('');
            $('#GenProdPostingGroup').val('');
        }
    });

    $("#BaseUnitOfMeasure").val('SQM').trigger('change');
    $("#SalesUnitOfMeasureWithSMPLGrade").val('PCS').trigger('change');
    $("#SalesUnitOfMeasureWithOtherGrade").val('BOX').trigger('change');
});

