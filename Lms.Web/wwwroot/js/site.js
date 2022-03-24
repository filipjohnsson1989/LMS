﻿$(document).ready(function () {
    //Course Selector 
    $('#selectedCourse').on('change', function (e) {
        changeTrackedCourse(this.value);
        setTimeout(5000);
        location.reload();
    });
});

//Course Selector returns course id
function changeTrackedCourse(id) {
    $.ajax({
        type: "GET",
        url: 'https://localhost:7290/Courses/TrackedCourseId/' + id,
        data: { "number": id },
        contentType: "application/json; charset=utf-8",
        success: function (r) {
            //alert(r);
        },
        error: function (r) {
            alert(r.responseText);
        }
    })
}