import { send } from "./_utils";
let submitButton = document.getElementById("submit");
submitButton.onclick = async function () {
    console.log("submitted, tries to add");
    let existsDiv = document.getElementById("UserExists");
    while (true) {
        let username = document.getElementById("signUser");
        let password = document.getElementById("signPass");
        let confirmPass = document.getElementById("signConfirm");
        if (password != confirmPass) {
            existsDiv.innerText = "password and confirm password dont match";
            continue;
        }
        break;
    }

    let user = {
        username: username,
        password: password
    };

    let Id = await send("/logIn", user);
    console.log("Sent to server");
    if (Id == "UserAlreadyExists") {
        existsDiv.innerHTML = 'username already exists, change it or log in <a href="logIn.html">HERE</a>';
    }

    return Id;
}