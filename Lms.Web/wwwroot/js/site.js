////$(window).on("load", function () {
////    //Course Selector 
    
////});
//$(document).ready(function () {
//    displaycourses();
//});

$('#selectedCourse').on('change', function (e) {
    changeTrackedCourse(this.value);
    
});

//function displaycourses() {
//    var courses = [];
//    $.ajax({
//        type: "GET",
//        url: "/Courses/GetCourses",
//        success: function (data) {
//            $.each(data, function (index, item) {
//                courses.push({
//                    title: item.name,
//                    color: "#" + Math.floor(Math.random() * 16777215).toString(16),
//                });
//            })

//            GenerateCourseTextArea(courses);
//        },
//        error: function (error) {
//            alert('failed');
//        }
//    })
//}

//Course Selector returns course id
function changeTrackedCourse(id) {
  
    $.ajax({
        url: '/CourseSelector/TempData/',
        data: {id: id},
        type: 'POST',
        success: function (data) { window.location.href = '../' },
        error: function (r) {
            alert(r.responseText);
        }

    });


}