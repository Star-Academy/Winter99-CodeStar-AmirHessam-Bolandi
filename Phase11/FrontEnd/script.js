var url = "https://localhost:5001/"

function initSearch() {
    // document.getElementById("advancedInputs").style.display = "none";
    document.getElementById("advancedInputs").className = "none";


    // const initrequest = new XMLHttpRequest();
    // initrequest.onreadystatechange = function () {
    //     if (this.readyState === 4 && this.status === 200) {
    //         document.getElementById("resultBox").innerHTML = this.status+"{"+this.responseText+"}";
    //     }
    // };
    // initrequest.open('GET', 'https://localhost:5001/init/y');
    // initrequest.responseType = 'text';
    // initrequest.send();
}

function preSearch() {
    if (advanceActive)
        return;
    let query = document.getElementById("searchInput").value.trim();
    if (query === "")
        return;
    // document.getElementById("resultBox").innerHTML = query + " query";
    const xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            const resultsElement = document.createElement("div");
            resultsElement.className = "results";
            let parsed = JSON.parse(this.responseText);

            for (let i = 0; i < 10 && i < parsed.length; i++) {
                const divElement = document.createElement("div");
                divElement.className = "result";
                divElement.innerHTML = parsed[i];
                resultsElement.appendChild(divElement);
            }
            document.getElementById("resultBox").innerHTML = "نتایج:";
            document.getElementById("resultBox").appendChild(resultsElement);
        }
    };
    xhttp.open('GET', url + 'query/' + query);
    xhttp.responseType = 'text';
    xhttp.send();
}

function search() {
    let normQuery = document.getElementById("searchInput").value.trim();
    let plusQuery = document.getElementById("plusInput").value.trim();
    let minusQuery = document.getElementById("minusInput").value.trim();

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


    const xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            const resultsElement = document.createElement("div");
            resultsElement.className = "results";
            let parsed = JSON.parse(this.responseText);

            for (let i = 0; i < 10 && i < parsed.length; i++) {
                const divElement = document.createElement("div");
                divElement.className = "result";
                divElement.innerHTML = parsed[i];
                resultsElement.appendChild(divElement);
            }
            document.getElementById("resultBox").innerHTML = "نتایج:";
            document.getElementById("resultBox").appendChild(resultsElement);
        }
    };


    xhttp.open('GET', url + queryString);
    xhttp.responseType = 'text';
    xhttp.send();
}

var advanceActive = false;

function advancedButtonHandler() {
    if (!advanceActive) {
        document.getElementById("resultBox").innerHTML = " ";

        // document.getElementById("advancedInputs").style.display= "inline";
        document.getElementById("advancedInputs").className = "advanced-inputs";
        document.getElementById("searchOptionButton").style.fontSize = "15px";
        document.getElementById("searchOptionButton").innerHTML = "جست و جو"
        document.getElementById("searchBoxLabel").innerHTML = "عبارات مورد نظر حود را با فاصله وارد کنید:"

        document.getElementById("normalBox").className = ".advanced-boxes";
        // document.getElementById("normalBox").style.gridTemplateRows= "90px 90px auto";

        advanceActive = true;
    } else {
        search();
    }

}
