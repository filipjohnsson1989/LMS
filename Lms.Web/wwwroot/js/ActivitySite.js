const btn = document.querySelectorAll('span');


btn.forEach(btn => btn.addEventListener('click', function () {
    let parent = this.parentNode;
    let element = parent.nextElementSibling;
    if (element.classList.contains('hidden')) {
        element.classList.remove('hidden');
    }
    else {element.classList.add('hidden') }
    

    /*console.log(element);*/
}));




