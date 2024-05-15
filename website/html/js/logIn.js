import { send } from "./_utils";
async function getIdFromLogIn() {
    let username = document.getElementById("logUser");
    let password = document.getElementById("logPass");
    let user = {
        username: username,
        password: password
    };
    let Id = await send("/logIn", user);
    if (Id == "UserDoesntExist") {
        // here
    }
    return Id;
}
console.log(getIdFromLogIn());