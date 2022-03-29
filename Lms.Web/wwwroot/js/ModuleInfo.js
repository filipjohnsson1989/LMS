var moduleInfo = document.querySelectorAll(".infoBtn");

moduleInfo.forEach(c => c.addEventListener('click', function (info) {
    let nextElement = info.target.nextElementSibling.nextElementSibling;
    if (nextElement.classList.contains("hidden"))
        nextElement.classList.remove("hidden");
    else
        nextElement.classList.add("hidden");
}));

//var moduleBtn = document.querySelectorAll(".moduleSelect");

//moduleBtn.forEach(c => c.addEventListener('click', function () {

//    moduleBtn.forEach(b => b.classList.remove("active"));

//    if (!this.classList.contains("active"))
//        this.classList.add("active");
    
    
//})); this code won't work on refresh. 