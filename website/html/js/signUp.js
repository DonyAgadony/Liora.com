import Cookies from "./_cookies";
import { send } from "./_utils";
let submitButton = document.getElementById("submit");
submitButton.onclick = async function () {
    let password = document.getElementById("signPass");
    let confirmPass = document.getElementById("signConfirm");
    let existsDiv = document.getElementById("UserExists");
    let username = document.getElementById("signUser");
    if (password.value != confirmPass.value) {
        existsDiv.innerText = "password and confirm password dont match";
    }
    else if(password.value ==null) {
          existsDiv.innerText = "password must have character";}
    else {

        let user = {
            username: username.value,
            password: password.value
        };

        let Id = await send("/signUp", user);
        if (Id == "UserAlreadyExists") {
            existsDiv.innerHTML = 'username already exists, change it or log in <a href="logIn.html">HERE</a>';
        }
        Cookies.set("Id", Id);
        Cookies.set("username", username.value);
        window.location.href = "index.html";
    }
}