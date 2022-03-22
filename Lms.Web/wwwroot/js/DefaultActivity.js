
$(document).ready(start());

//start function
//    if url is activity/any id
//         do nothing
//   else go to url/activity/model id

function start() {
    if (window.location.pathname == '/')
        window.location.pathname = '/' + 'Activities' + '/' + id;

}
