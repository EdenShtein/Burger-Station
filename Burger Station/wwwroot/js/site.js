// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Post using facebook api

const token = "EAAkY2kcKeuQBACLLC5fGZBhUZAxIOiZASOKEDKrQnfcYWZAhQ4jZBKvXIU5IOG3oDCMzhoBI6mw2IzNrtdCo07kdVphZAX6GXMHPz9oD8g4ANJWTMLj70rNgKn1ot3yC3YDeRDXOq4vAelDGbemT5Sq0Fr7BWWv1y29lppK6o10wZDZD";


function postAStatus() {
    var status = document.getElementById('postTxt').value;

    FB.api(
        '/112744500542236/feed',
        'POST',
        {
            "message": status,
            "access_token": token
        },
        function (response) {
            console.log(response);
        }
    );
}

//function boostAProduct(status, photoURL) {
//    FB.api(
//        '/101290241700621/photos',
//        'POST',
//        {
//            "message": status,
//            "url": photoURL,
//            "access_token": token
//        },
//        function (response) {
//            console.log(response);
//        }
//    );
//}