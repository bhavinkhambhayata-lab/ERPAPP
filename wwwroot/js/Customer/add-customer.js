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

                console.log(data); // 🔎 check JSON structure

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

        $("#Name").val($(this).text());
        $("#CustomerNo").val($(this).data("no"));
        $("#customerSearchResult").html("");

    });

});