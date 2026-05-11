$(document).ready(function () {

    $('#GSTGroupCode').change(function () {

        var gstCode = $(this).val();

        $('#HSNSACCode').empty();
        $('#HSNSACCode').append('<option value="">--Select--</option>');

        if (gstCode !== '') {

            $.ajax({
                url: baseURL + 'FGItem/GetFixedAssetHSNDataWithGSTGroupCode',
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

        // Main Company Code
        let itemCode = "FG1213";

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
                                       class="form-control"
                                       value="${currentItemCode}"
                                       readonly />
                            </td>

                            <td>
                                <input type="text"
                                       class="form-control"
                                       value="${selectedGrades[i].grade}"
                                       readonly />
                            </td>

                           <td>
                                <input type="text"
                                       class="form-control"
                                       value="${
                                            commonGradeLinkCode.replace(
                                                /\.\d+$/,
                                                selectedGrades[i].gradeLinkCode
                                            )
                                        }" readonly />
                            </td>

                        </tr>`;
        }

        $("#grade-tbbody").html(tbody);

    });
});