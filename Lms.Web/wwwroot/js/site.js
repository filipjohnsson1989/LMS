
    $('#selectedCourse').on('change', function (e) {
        changeTrackedCourse(this.value);
    });

    //Course Selector returns course id
    function changeTrackedCourse(id) {

        $.ajax({
            url: '/CourseSelector/TempData/',
            data: { id: id },
            type: 'POST',
            success: function (data) { window.location.href = '../' },
            error: function (r) {
                alert(r.responseText);
            }
        });
    }

    
    //coursecalender

    $('#coursetimeline').click(function () {
        var events = [];
        $.ajax({
            type: "GET",
            url: "/Courses/CourseTimeLine",
            success: function (data) {


                $.each(data, function (index, item) {
                    events.push({
                        
                        title: item.name,
                        start: moment(item.startDate),
                        end: moment(item.startDate),
                        color: "#" + Math.floor(Math.random() * 16777215).toString(16),
                    });
                })

                GenerateCalender(events);
            },
            error: function (error) {
                alert('failed');
            }
        })
    });

//modulecalender

$('#moduletimeline').click(function () {
    var events = [];
    $.ajax({
        type: "GET",
        url: "/Modules/ModuleTimeLine/",
        success: function (data) {


            $.each(data, function (index, item) {
                events.push({

                    title: item.name,
                    start: moment(item.startDate),
                    end: moment(item.endDate),
                    color: "#" + Math.floor(Math.random() * 16777215).toString(16),
                });
            })

            GenerateCalender(events);
        },
        error: function (error) {
            alert('failed');
        }
    })
});


function GenerateCalender(events) {
        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            contentHeight: 350,
            defaultDate: new Date(),

            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay,agenda'
            },
            eventLimit: true,

            events: events,
            eventClick: function (calEvent) {
                eventColor: calEvent.color,
                    $('#myModal #eventTitle').text(calEvent.title);
                var $description = $('<div/>');
                $description.append($('<p/>').html('<b>Start:</b>' + calEvent.start.format("DD-MMM-YYYY ")));
                if (calEvent.end != null) {
                    $description.append($('<p/>').html('<b>End:</b>' + calEvent.end.format("DD-MMM-YYYY ")));
                }
                $description.append($('<p/>').html('<b>Description:</b>'));
                $('#myModal #pDetails').empty().html($description);

                $('#myModal').modal();
            }
        });
    }
//});