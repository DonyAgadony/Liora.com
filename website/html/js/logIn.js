import { send } from "./_utils";
async function getIdFromLogIn() {
  let username = document.getElementById("logUser").value;
  let password = document.getElementById("logPass").value;
  let user = {
    username: username,
    password: password,
  };
  let Id = await send("/logIn", user);
  if (Id == "UserDoesntExist" || Id == "IncorrectPassword") {
    let doesntExistDiv = document.getElementById("exists");
    doesntExistDiv.innerHTML = 'Incorrect username or password, or sign up <a href="signUp.html">HERE</a>';
  }
  return Id;
}
console.log(getIdFromLogIn());