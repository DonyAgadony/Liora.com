import { send } from "./_utils";
async function AddUserToDB() {
    while (true) {
        let username = document.getElementById("signUser");
        let password = document.getElementById("signPass");
        let user = {
            username: username,
            password: password
        };
        let Id = await send("/logIn", user);
        if (Id == "UserAlreadyExists") {
            // here
            continue;
        }
        break;
    }
    return Id;
}