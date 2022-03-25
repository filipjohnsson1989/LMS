////$(window).on("load", function () {
////    //Course Selector 
    
////});
$('#selectedCourse').on('change', function (e) {
    changeTrackedCourse(this.value);
    
});
//Course Selector returns course id
function changeTrackedCourse(id) {
  
    $.ajax({
        url: '/indices/TestMethod/',
        data: {id: id},
        type: 'POST',
        success: function (data) { location.reload();}
    });


}