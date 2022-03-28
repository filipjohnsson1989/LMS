var allModules = document.querySelectorAll(".moduleClickable");
var plus = "+";
var minus = "-";

allModules.forEach(btn => btn.addEventListener('click', function (event) {
    let plusSign = event.target;
    let parent = plusSign.parentNode;
    let child = parent.children;

    for (var i = 0; i < child.length; i++)
    {
        if (child[i].classList.contains('expandState'))
        {
            if (child[i].innerText == plus)
                child[i].innerText = minus;
            else
                child[i].innerText = plus;
        }
    }

    let targetId = event.target.parentNode.id;
    
    let hidden = document.querySelectorAll(`.${targetId}`);

    hidden.forEach(function (element) {
        if (element.classList.contains('hidden')) {
            element.classList.remove('hidden');
        }
        else { element.classList.add('hidden') }

    })
}));




