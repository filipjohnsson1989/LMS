var allModules = document.querySelectorAll(".moduleClickable");

allModules.forEach(btn => btn.addEventListener('click', function (event) {
    console.log('click');
    let targetId = event.target.parentNode.id;
    
    let hidden = document.querySelectorAll(`.${targetId}`);
    console.log(hidden);

    hidden.forEach(function (element) {
        if (element.classList.contains('hidden')) {
            element.classList.remove('hidden');
        }
        else { element.classList.add('hidden') }

    })
}));




