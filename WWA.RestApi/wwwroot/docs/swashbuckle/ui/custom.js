function when_loaded(callback) {
    var logoAndHeader = document.querySelector("a.link");
    if (logoAndHeader === null || logoAndHeader === undefined) {
        setTimeout(function () {
            when_loaded(callback)
        }, 20);
    }
    else {
        callback(logoAndHeader);
    }
}

when_loaded(function (logoAndHeader) {

    document.title = "World Warriors Arena REST API";

    while (logoAndHeader.hasChildNodes()) {
        logoAndHeader.removeChild(logoAndHeader.lastChild);
    }

    var logo = document.createElement("IMG");
    logo.alt = "World Warriors Arena";
    logo.src = "./swashbuckle/ui/WWA-72x80.png";
    logo.width = 40;

    var header = document.createElement("SPAN");
    header.innerText = "World Warriors Arena"

    logoAndHeader.appendChild(logo);
    logoAndHeader.appendChild(header);
    logoAndHeader.href = "#";

});