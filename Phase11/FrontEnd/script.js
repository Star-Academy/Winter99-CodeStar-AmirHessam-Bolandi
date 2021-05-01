const url = "https://localhost:5001/"
var normQuery;
var plusQuery;
var minusQuery;
var queryWords = [];

function initSearch() {
    document.getElementById("advancedInputs").className = "none";

    const initrequest = new XMLHttpRequest();
    initrequest.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            document.getElementById("searchBoxLabel").innerHTML = "عبارت را وارد کنید :";
        }else if(this.readyState === 4 && this.status !== 200){
            document.getElementById("resultBox").innerHTML = "متاسفانه خطایی رخ داده و قادر به ارائه خدمات نیستیم:( "+this.responseText+"{"+this.status+"}";
        }
    };
    initrequest.open('PUT', 'https://localhost:5001/init/y');
    initrequest.responseType = 'text';
    initrequest.send();
}

function preSearch() {

    let query = document.getElementById("searchInput").value.trim();
    if (query === "" || advanceActive)
        return;
    normQuery = query;
    queryWords = normQuery.split(" ");
    let xhttp = searchQueryHandler();
    xhttp.open('GET', url + 'query/' + normQuery);
    xhttp.responseType = 'text';
    xhttp.send();
}

function advanceSearch() {
    normQuery = document.getElementById("searchInput").value.trim();
    plusQuery = document.getElementById("plusInput").value.trim();

    if (normQuery + plusQuery + minusQuery === "")
        return;
    let parameters = [];
    if (normQuery !== "")
        parameters.push('normals="' + normQuery + '"');
    if (minusQuery !== "")
        parameters.push('minuses="' + minusQuery + '"');
    if (plusQuery !== "")
        parameters.push('pluses="' + plusQuery + '"');
    let queryString = 'query?' + parameters.join("&")

    queryWords = normQuery.split(" ");
    queryWords.push(plusQuery.split(" "));
    // queryWords.push(minusQuery.split(" "));

    let xhttp = searchQueryHandler();
    xhttp.open('GET', url + queryString);
    xhttp.responseType = 'text';
    xhttp.send();
}

var advanceActive = false;

function advancedButtonHandler() {
    if (!advanceActive) {
        document.getElementById("resultBox").innerHTML = "";

        // document.getElementById("advancedInputs").style.display= "inline";
        document.getElementById("advancedInputs").className = "advanced-inputs";
        document.getElementById("searchOptionButton").style.fontSize = "15px";
        document.getElementById("searchOptionButton").innerHTML = "جست و جو"
        document.getElementById("searchBoxLabel").innerHTML = "عبارات مورد نظر خود را با فاصله وارد کنید:"

        document.getElementById("normalBox").className = ".advanced-boxes";
        // document.getElementById("normalBox").style.gridTemplateRows= "90px 90px auto";
        advanceActive = true;
    } else {
        advanceSearch();
    }

}

function fileHandler(element) {
    let fileName = element.target.id;

    const xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            let parsed = JSON.parse(this.responseText);
            if (parsed == []) {

            } else {
                let resultElement = document.getElementById("resultBox");
                resultElement.innerHTML = "";

                let divElement = document.createElement("div");
                divElement.className = "file-name";
                divElement.innerHTML = parsed[0].DocumentId;
                resultElement.appendChild(divElement);
                divElement = document.createElement("div");
                divElement.className = "scroll-box";
                var content =  parsed[0].Content;
                for (let i = 0; i < queryWords.length; i++) {
                    if(queryWords[i]=="")
                        continue;
                    var wordRegex=new RegExp(queryWords[i], "ig");
                    content =content.replaceAll(wordRegex,'<span class="highlighted-word">'+queryWords[i]+'</span>');
                }
                divElement.innerHTML =content;
                resultElement.appendChild(divElement);

            }
        }
    };

    xhttp.open('GET', url + "file/" + fileName);
    xhttp.responseType = 'text';
    xhttp.send();
}

function searchQueryHandler() {
    const xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            let parsed = JSON.parse(this.responseText);
            if(parsed.length==0){
                document.getElementById("resultBox").innerHTML = "نتیجه ای یافت نشد!<br>"
                return;
            }
            const resultsElement = document.createElement("div");
            resultsElement.className = "results";
            for (let i = 0; i < 10 && i < parsed.length; i++) {
                const divElement = document.createElement("div");
                divElement.id = parsed[i];
                divElement.addEventListener('click', fileHandler, false);
                divElement.className = "result";
                divElement.innerHTML = parsed[i];
                resultsElement.appendChild(divElement);
            }

            document.getElementById("resultBox").innerHTML = "نتایج:<br>"
            document.getElementById("resultBox").appendChild(resultsElement);
        }
    };
    return xhttp;
}

