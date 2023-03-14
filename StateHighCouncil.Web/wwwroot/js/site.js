// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ToggleTracking(billId) {
    (async () => {
        const response = await fetch('/Api/ToggleBillTracking/?billid=' + billId);
        
        const data = await response.json();
        
        var button = "button" + billId;
        var badge = "badge" + billId;
        var icon = "icon" + billId;

        if (data === true) {
            document.getElementById(badge).classList.remove("bg-secondary");
            document.getElementById(icon).classList.remove("fa-circle-plus");
            document.getElementById(badge).classList.add('bg-primary');
            document.getElementById(icon).classList.add('fa-circle-check');
        }
        else {
            document.getElementById(badge).classList.remove('bg-primary');
            document.getElementById(icon).classList.remove('fa-circle-check');
            document.getElementById(badge).classList.add("bg-secondary");
            document.getElementById(icon).classList.add("fa-circle-plus");
        }
    })();
}

function loadSessions() {
    var select = document.getElementById("sessionSelector");
    var option = document.createElement('option');
    option.value = "2023GS";
    option.text = "2023 General Session";
    option.className = "dropdown-item";
    select.add(option);

    option = document.createElement('option');
    option.value = "2022GS";
    option.text = "2022 General Session";
    option.ClassName = "dropdown-item";
    select.add(option);
}

function setSession() {
    var select = document.getElementById("sessionSelector");
    var value = select.value;
    location.reload(true);
}