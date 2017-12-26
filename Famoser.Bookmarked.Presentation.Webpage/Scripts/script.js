async function sha256(message) {
    const msgBuffer = new TextEncoder('utf-8').encode(message);                     // encode as UTF-8
    const hashBuffer = await crypto.subtle.digest('SHA-256', msgBuffer);            // hash the message
    const hashArray = Array.from(new Uint8Array(hashBuffer));                       // convert ArrayBuffer to Array
    const hashHex = hashArray.map(b => ('00' + b.toString(16)).slice(-2)).join(''); // convert bytes to hex string
    return hashHex;
}

$(document).ready(function () {
    $('#guid').val(localStorage.getItem("guid"));

    $('form').on(
        "submit",
        function(e) {
            e.preventDefault();
            return false;
        }
    );
});

function bootApplication() {
    const $val = $("#password").val();
    const $guid = $("#guid").val();
    (async function (guid) {
        const hash = await sha256($val);
        sessionStorage.setItem("password", hash);
        localStorage.setItem("guid", guid);
        window.location = "View/Navigate/" + guid;
    }($guid));
}
