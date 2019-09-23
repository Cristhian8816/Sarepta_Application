//section to build JWT
var token = "";
var userName = "DraAlma";
var timeStamp = Date.now();
var date = new Date(timeStamp).toDateString().replace(/ /g, "");
var header = {
    "alg": "HS256",
    "typ": "JWT"
};

var data = {
    "id": userName + date
};

var secret = userName;

var stringifiedHeader = CryptoJS.enc.Utf8.parse(JSON.stringify(header));
var encodedHeader = base64url(stringifiedHeader);
token += encodedHeader + ".";

var stringifiedData = CryptoJS.enc.Utf8.parse(JSON.stringify(data));
var encodedData = base64url(stringifiedData);
token += encodedData + ".";

var signature = encodedHeader + "." + encodedData;
signature = CryptoJS.HmacSHA256(signature, secret);
signature = base64url(signature);

token += signature;

console.log(token);

var x = document.getElementById("token");

x.value = token;
function base64url(source) {
    // Encode in classical base64
    encodedSource = CryptoJS.enc.Base64.stringify(source);

    // Remove padding equal characters
    encodedSource = encodedSource.replace(/=+$/, '');

    // Replace characters according to base64url specifications
    encodedSource = encodedSource.replace(/\+/g, '-');
    encodedSource = encodedSource.replace(/\//g, '_');

    return encodedSource;
}
