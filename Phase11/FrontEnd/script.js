

function initSearch() {
    // const initrequest = new XMLHttpRequest();
    // initrequest.onreadystatechange = function () {
    //     if (this.readyState === 4 && this.status === 200) {
    //         document.getElementById("result_box").innerHTML = this.status+"{"+this.responseText+"}";
    //     }
    // };
    // initrequest.open('GET', 'https://localhost:5001/init/y');
    // initrequest.responseType = 'text';
    // initrequest.send();
}

function preSearch() {
    let query = document.getElementById("searchInput").value;
    document.getElementById("result_box").innerHTML = query + " query";
    const xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (this.readyState === 4 && this.status === 200) {
            const resultsElement = document.createElement("div");
            resultsElement.className = "results";
            var responseStr = this.responseText;
            responseStr = responseStr.substr(1,responseStr.length-2);
            var responselist = responseStr.split(",")




            for (let i = 0; i < 10 && i<responselist.length; i++) {
                const divElement = document.createElement("div");
                divElement.className = "result";
                divElement.innerHTML = responselist[i].replaceAll('"','');
                resultsElement.appendChild(divElement);
            }
            document.getElementById("result_box").innerHTML = "نتایج:"+responseStr;
            document.getElementById("result_box").appendChild(resultsElement);
        }
    };
    xhttp.open('GET', 'https://localhost:5001/query/'+query);
    xhttp.responseType = 'text';
    xhttp.send();
}

