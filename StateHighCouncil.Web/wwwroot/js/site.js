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

function setStatusFilter() {
    var select = document.getElementById("statusSelector");
    var value = select.value;
    alert("Setting cookie value: " + value);
    setCookie("statusFilter", value, 1);
    location.reload(true);
}

function setCurrentStatus() {
    const cookieValue = getCookie("statusFilter");
    alert("Setting status: " + cookieValue);
    const select = document.getElementById("statusSelector");
    select[select.selectedIndex].removeAttr("selected");
    for (var i = 0; i < select.options.length; i++) {
        if (select.options[i] == cookieValue) {
            select.selectedIndex = i;            
        }
    }
}

function loadSubjects() {
    (async () => {
        var selectSubject = '@ViewBag.SubjectValue';
        const response = await fetch('/Api/Subjects');
        const data = await response.json();
        const statusValue = '@ViewBag.StatusValue';

        var selector = document.getElementById("subjectSelector");
        selector.options.length = 0;

        var option = document.createElement("option");
        option.text = "All";
        selector.add(option);

        for (var i = 0; i < data.length; i++) {
            option = document.createElement("option");
            option.text = data[i].name;
            if (data[i].name == statusValue) {
                option.selected = true;
            }
            selector.add(option);
        }
    })()
}

function clearStatus(billId) {
    (async () => {
        const response = await fetch('/Api/ClearStatus?billId='+billId);
        const data = await response.json();
        
        if (data) {
            const shouldRemoveDiv = '@ViewBag.ShouldRemoveDiv'
            
            if (shouldRemoveDiv) {
                element = document.getElementById("div-" + billId);
            }
            else {
                element = document.getElementById('badge' + item.Id);
            }
            element.remove();
        }
    })()
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