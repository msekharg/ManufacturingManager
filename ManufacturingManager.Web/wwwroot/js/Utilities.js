function timerInactive(dotnetHelper) {
    var timer;
    var timeout = 3; //minutes 15;
    var timeoutAlert = 2;
    var newWindow;
    document.onmousemove = resetTimer;
    document.onkeypress = resetTimer;
    document.body.style.backgroundColor = "yellow";
    function resetTimer() {
        clearTimeout(timer);
        //timer = setTimeout(alertTimeout, (timeout - timeoutAlert) *  1000); 
        timer = setTimeout(openWindow, 3000);  //3 seconds
    }
    function alertTimeout() {
        clearTimeout(timer);
        timer = setTimeout(logout, 3000);
        document.body.style.backgroundColor = "red";

        //popup showing the message and timer 
        // var popupMsg = '<div><p>Timeout in 1 minute</p></div>';

        //$(popupMsg).dialog();
        //alert('timeout in 1 min');

    }

    function logout() {
        document.body.style.backgroundColor = "blue";

        dotnetHelper.invokeMethodAsync("Logout");
    }


    function openWindow() {
        clearTimeout(timer);
        timer = setTimeout(function () {

            newWindow.close();
            logout();
        }, 30000);
        document.body.style.backgroundColor = "red";
        var i, l, options = [{
            value: 'first',
            text: 'First'
        }, {
            value: 'second',
            text: 'Second'
        }],
            newWindow = window.open("", null, "height=200,width=400,status=yes,toolbar=no,menubar=no,location=no");

        newWindow.document.write("<select onchange='window.opener.setValue(this.value);'>");
        for (i = 0, l = options.length; i < l; i++) {
            newWindow.document.write("<option value='" + options[i].value + "'>");
            newWindow.document.write(options[i].text);
            newWindow.document.write("</option>");
        }
        newWindow.document.write("</select>");
        newWindow.document.write("<br>Timer: ");
        newWindow.document.createElement("lblTimer");
        //newWindow.document.getElementById("lblTimer")
        var elem2 = document.createElement('label');
        elem2.setAttribute('id', 'lblName');
        elem2.innerHTML = "Some value";
        // newWindow.document.appendChild(elem2);
        newWindow.document.getElementsByTagName('body')[0].appendChild(elem2);
        newWindow.document.getElementById('lblName').innerHTML = 'Hi, I am tester';

        // setInterval(myTimer(newWindow), 1000);
        setInterval(function () {
            d = new Date();
            newWindow.document.getElementById("lblName").innerHTML = d.toLocaleTimeString();
        }, 1000)

    }


    function myTimer(newWindow) {
        const d = new Date();
        newWindow.document.getElementById("lblName").innerHTML = d.toLocaleTimeString();
    }

    function setValue(value) {
        document.getElementById('value').value = value;
    }

}

function focusElement(element) {
    //debugger;
    if (document.getElementById(element) !== null) {
        document.getElementById(element).focus();
    }

}
function redirect(path) {
    window.location = path;
}