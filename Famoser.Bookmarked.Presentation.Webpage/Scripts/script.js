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
        function (e) {
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

function setGuid() {
    const files = $("#file").files;
    readFile(files[0]);
}

function readFile(file) {
    const val = $("#password").val();

    const reader = new FileReader();

    // If we use onloadend, we need to check the readyState.
    reader.onloadend = function (evt) {
        if (evt.target.readyState == FileReader.DONE) {
            const bytes = evt.target.result;
            console.log(decryptText(bytes, val));
        }
    };
    reader.readAsArrayBuffer(file);

}

const testEncryption = async () => {
    var password = "mypassword";
    var payload = "hi mom";

    var encrypted = await encryptText(payload, password);
    var decrypted = await decryptText(encrypted, password);

    assert(payload === decrypted);
}

function assert(res) {
    if (res !== true) {
        throw "assertion failed";
    } else {
        console.log("(assertion succeeded)");
    }
}

function getIvArray() {
    const array = new Uint8Array(16);
    array[0] = 2;
    array[1] = 1;
    array[2] = 42;
    array[3] = 14;
    array[4] = 1;
    array[5] = 2;
    array[6] = 12;
    array[7] = 4;
    array[8] = 51;
    array[9] = 21;
    array[10] = 12;
    array[11] = 3;
    array[12] = 12;
    array[13] = 3;
    array[14] = 14;
    array[15] = 12;
    return array;
}

const encryptText = async (plainText, password) => {
    //create password hash
    const pwUtf8 = new TextEncoder().encode(password);
    const pwHash = await crypto.subtle.digest('SHA-256', pwUtf8);

    //encode plain text
    const ptUtf8 = new TextEncoder().encode(plainText);

    //create key
    const alg = { name: 'AES-CBC', iv: getIvArray() };
    const key = await crypto.subtle.importKey('raw', pwHash, alg, false, ['encrypt']);

    //encrypt
    return crypto.subtle.encrypt(alg, key, ptUtf8);
}


const decryptText = async (bytes, password) => {
    //create password hash
    const pwUtf8 = new TextEncoder().encode(password);
    const pwHash = await crypto.subtle.digest('SHA-256', pwUtf8);

    //create key
    const alg = { name: 'AES-CBC', iv: getIvArray() };
    const key = await crypto.subtle.importKey('raw', pwHash, alg, false, ['decrypt']);

    //decrypt
    const ptBuffer = await crypto.subtle.decrypt(alg, key, bytes);
    return new TextDecoder().decode(ptBuffer);
}