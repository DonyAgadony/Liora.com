import { send } from "./_utils";
async function addUserToDB() {
    // let username = document.getElementById("signUser");
    // let password = document.getElementById("signPass");
    let user = {
        username: "IdoToxido",
        password: "123"
    };
    let Id = await send("/logIn", user);
    console.log("Sent to server");
    if (Id == "UserAlreadyExists") {
        let existsDiv = document.getElementById("UserExists");
        existsDiv.innerHTML = 'username already exists, change it or log in <a href="logIn.html">HERE</a>';
    }
     
    return Id;
}