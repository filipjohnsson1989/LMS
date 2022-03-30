const btn = document.querySelectorAll(".infoButton");


btn.forEach(btn => btn.addEventListener('click', function () {
    let infoCard = this.nextElementSibling;
    

        if (infoCard.classList.contains("expandCard"))
        {
                changeBtn(this);
            let expando = infoCard.classList;
            if (expando.contains("hidden")) {
                expando.remove("hidden");
            }
            else {
                expando.add("hidden");
            }
        }
}));


function changeBtn(infoCard)
{
    console.log(infoCard);
    let children = infoCard.children;
    for (var i = 0; i < children.length; i++)
    {
        if (children[i].classList.contains("expandState"))
        {
            let expandoToken = children[i];
            if (expandoToken.innerText == "+")
                expandoToken.innerText = "-";
            else
                expandoToken.innerText = "+";
        }
    }
}

