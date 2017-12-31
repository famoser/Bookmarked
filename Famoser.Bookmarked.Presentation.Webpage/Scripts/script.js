$(document).ready(function () {
    $('#guid').val(localStorage.getItem("guid"));
    $('#password').val(sessionStorage.getItem("password"));

    $('form').on(
        "submit",
        function (e) {
            e.preventDefault();
            return false;
        }
    );
});

function bootApplication() {
    const $password = $("#password").val();
    const $guid = $("#guid").val();
    (async function (guid, password) {
        const hash = await getPasswordHash(password);
        sessionStorage.setItem("password_hash", hash);
        sessionStorage.setItem("password", password);
        localStorage.setItem("guid", guid);
        window.location = "View/Navigate/" + guid;
    }($guid, $password));
}

function setGuid() {
    const file = document.getElementById('file').files[0];

    const reader = new FileReader();
    reader.onloadend = function (evt) {
        if (evt.target.readyState === FileReader.DONE) {
            const bytes = evt.target.result;
            decryptFile(bytes);
        }
    };

    var blob = file.slice(0, file.length);
    reader.readAsText(blob);
}

decryptFile = async (base64) => {
    const val = $("#password").val();

    //decrypt base64
    var binaryString = window.atob(base64);
    var length = binaryString.length;
    var bytes = new Uint8Array(length);
    for (var i = 0; i < length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }

    //decrypt cypher text
    const text = await decryptText(bytes.buffer, val);
    console.log(text);
    $("#guid").val(text);
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

const getPasswordHash = async (password) => {
    //get hash
    const pwUtf8 = new TextEncoder().encode(password);
    const hash = await crypto.subtle.digest('SHA-256', pwUtf8);

    //apply Pkcs7 padding
    var maxSize = 256 / 8;
    var padSize = maxSize - (hash.byteLength % maxSize);
    var padCode = padSize;
    var res = new ArrayBuffer(maxSize);
    for (var i = hash.byteLength; i < res.length; i++) {
        res[i] = padCode;
    }
    return res;
}

const encryptText = async (plainText, password) => {
    //create password hash
    const pwHash = await getPasswordHash(password);

    //encode plain text
    const ptUtf8 = new TextEncoder().encode(plainText);

    //create key
    const alg = { name: 'AES-CBC', iv: getIvArray() };
    const key = await crypto.subtle.importKey('raw', pwHash, alg, false, ['encrypt']);

    //encrypt
    return crypto.subtle.encrypt(alg, key, ptUtf8);
}


const decryptText = async (bytes, password) => {
    try {
        //create password hash
        const pwHash = await getPasswordHash(password);

        //create key
        const alg = { name: 'AES-CBC', iv: getIvArray() };
        const key = await crypto.subtle.importKey('raw', pwHash, alg, false, ['decrypt']);

        //decrypt
        const ptBuffer = await crypto.subtle.decrypt(alg, key, bytes);
        return new TextDecoder().decode(ptBuffer);
    } catch (e) {
        console.log(e);
    }
    return "failed";
}