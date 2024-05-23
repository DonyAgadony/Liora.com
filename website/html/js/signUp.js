import Cookies from "./_cookies";
import { send } from "./_utils";
let submitButton = document.getElementById("submit");
submitButton.onclick = async function () {
    console.log("submitted, tries to add");
    let password = document.getElementById("signPass");
    let confirmPass = document.getElementById("signConfirm");
    let existsDiv = document.getElementById("UserExists");
    let username = document.getElementById("signUser");
    if (password.value != confirmPass.value) {
        existsDiv.innerText = "password and confirm password dont match";
    }
    else {

        let user = {
            username: username.value,
            password: password.value
        };

        let Id = await send("/signUp", user);
        console.log("Sent to server");
        if (Id == "UserAlreadyExists") {
            existsDiv.innerHTML = 'username already exists, change it or log in <a href="logIn.html">HERE</a>';
        }
        console.log(Id);
        Cookies.set("Id", Id);
        Cookies.set("username", username.value);
        window.location.href = "index.html";
    }
}