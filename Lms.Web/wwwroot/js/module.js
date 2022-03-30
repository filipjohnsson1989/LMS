$(document).ready(function () {
    function setModule(id,label) {
        $("#Module_Id").val(id);
        $("#Module_Name").val(label);
    }

    $('#Module_Name').autocomplete({
        source: '/modules/search',
        minLength: 2,
        focus: function (event, ui) {
            $("#Module_Name").val(ui.item.name);
            return false;
        },
        success: function (data) {
            response($.map(data.d, function (item) {
                return {
                    label: item.name, // Set the display lebel for the searched list of module names which we're getting from server side coding.
                    value: item.id // Set the raw value of each shown items.
                }
            }));
        },
        select: function (event, ui) {
            setModule(ui.item.id, ui.item.name);
            return false;
        }
    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li>")
                .append("<div>Name:" + item.name + " Start:" + item.startDate + "</div>")
                .appendTo(ul);
        };
});