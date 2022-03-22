$(document).ready(function () {
    function setCourse(id,label) {
        $("#Course_Id").val(id);
        $("#Course_Name").val(label);
    }

    $('#Course_Name').autocomplete({
        source: '/courses/search',
        minLength: 2,
        focus: function (event, ui) {
            $("#Course_Name").val(ui.item.name);
            return false;
        },
        success: function (data) {
            response($.map(data.d, function (item) {
                return {
                    label: item.name, // Set the display lebel for the searched list of course names which we're getting from server side coding.
                    value: item.id // Set the raw value of each shown items.
                }
            }));
        },
        select: function (event, ui) {
            setCourse(ui.item.id, ui.item.name);
            return false;
        }
    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div>Name:" + item.name + " Start:" + item.startDate + "</div>")
                .appendTo(ul);
        };
});