$(document).ready(function () {

    let debounceTimer;

    $("#mobileSearchInput, #desktopSearchInput").on("input", function () {
        clearTimeout(debounceTimer);
        var searchText = $(this).val();

        debounceTimer = setTimeout(function () {
            if (searchText.length > 0) {
                $.ajax({
                    url: "/Home/SearchByTitle",
                    method: "GET",
                    data: { searchText: searchText },
                    success: function (response) {
                        $("#searchResults").html(response);
                        $("#searchResults").show();
                    },
                    error: function (error) {
                        console.log("Error in search:", error);
                    }
                });
            } else {
                $("#searchResults").hide();
            }
        }, 500);
    });

    // Hide search results when clicking outside
    $(document).on("click", function (event) {
        if (!$(event.target).closest("#searchResults, #mobileSearchInput, #desktopSearchInput").length) {
            $("#searchResults").hide();
        }
    });

});