
$(document).ready(start());

//start function
//    if url is activity/any id
//         do nothing
//   else go to url/activity/model id

function start() {
    if (id != null) {
        if (window.location.pathname == '/')
            window.location.pathname = '/' + 'LandingActivities' + '/' + id;
    }
}
