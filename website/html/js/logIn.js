import Cookies from "./_cookies";
import { send } from "./_utils";
let submit = document.getElementById("SubmitButton");
submit.onclick = async function () {
  let username = document.getElementById("logUser");
  let password = document.getElementById("logPass");
  var user = {
    username: username.value,
    password: password.value,
  };
  let Id = await send("/logIn", user);
  if (Id == "UserDoesntExist" || Id == "IncorrectPassword") {
    let doesntExistDiv = document.getElementById("exists");
    doesntExistDiv.innerText = "Incorrect username or password, or sign up";
    let signup = document.createElement("a");
    signup.href = "signUp.html";
    doesntExistDiv.appendChild(signup)
  }
  else {
    Cookies.set("Id", Id);
    Cookies.set("username", user.username);
    window.location.href = "index.html";
  }
}