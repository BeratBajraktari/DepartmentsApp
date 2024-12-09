function onErrorKendoGrid(e) {
    if (e.status == "customerror") {
        alert(e.errors);
    }
    else {
        alert("Generic server error.");
    }
}