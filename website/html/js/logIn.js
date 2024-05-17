import { send } from "./_utils";
export async function getIdFromLogIn() {
  let username = document.getElementById("logUser");
  let password = document.getElementById("logPass");
  var user = {
    username: username,
    password: password,
  };
  if (username == null) { var user = { username: "DonyAgadony", password: "Daniel123" }; }
  let Id = await send("/logIn", user);
  if (Id == "UserDoesntExist" || Id == "IncorrectPassword") {
    let doesntExistDiv = document.getElementById("exists");
    doesntExistDiv.innerHTML = 'Incorrect username or password, or sign up <a href="signUp.html">HERE</a>';
  }
  return Id;
}
console.log(getIdFromLogIn());