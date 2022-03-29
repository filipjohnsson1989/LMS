var moduleInfo = document.querySelectorAll(".infoBtn");

moduleInfo.forEach(c => c.addEventListener('click', function (info) {
    let nextElement = info.target.nextElementSibling.nextElementSibling;
    if (nextElement.classList.contains("hidden"))
        nextElement.classList.remove("hidden");
    else
        nextElement.classList.add("hidden");
}));