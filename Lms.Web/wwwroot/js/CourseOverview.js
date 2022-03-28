const links = document.querySelectorAll(".card-link");

links.forEach(btn => btn.addEventListener('click', function (event) {
    let previous = document.querySelectorAll(".active");
    console.log(previous);
    previous.forEach(e => e.classList.remove('active'));
    let target = event.target;
    if (!target.classList.contains('active')) {
        target.classList.add('active');
    }
}));




