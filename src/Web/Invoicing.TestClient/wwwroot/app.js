(function app() {
  var config, userManager;
  init();

  function init() {
    document.getElementById("login").addEventListener("click", login, false);
    document.getElementById("api").addEventListener("click", api, false);
    document.getElementById("logout").addEventListener("click", logout, false);

    config = {
      authority: "https://localhost:5000",
      client_id: "invoicing.test",
      redirect_uri: "https://localhost:5010/callback.html",
      response_type: "code",
      scope: "openid profile customer.api",
      post_logout_redirect_uri: "https://localhost:5010/index.html",
    };

    userManager = new Oidc.UserManager(config);

    userManager.getUser().then(function (user) {
      if (user) {
        log("User logged in", user.profile);
      } else {
        log("User not logged in");
      }
    });
  }

  function log() {
    document.getElementById("results").innerText = "";

    Array.prototype.forEach.call(arguments, function (msg) {
      if (msg instanceof Error) {
        msg = "Error: " + msg.message;
      } else if (typeof msg !== "string") {
        msg = JSON.stringify(msg, null, 2);
      }
      document.getElementById("results").innerHTML += msg + "\r\n";
    });
  }

  function login() {
    userManager.signinRedirect();
  }

  function api() {
    userManager.getUser().then(function (user) {
      
      var xhr = new XMLHttpRequest();
      xhr.open("POST", "https://localhost:5020/customer/company");
      xhr.onload = function () {
        log(xhr.status, JSON.parse(xhr.responseText));
      };
      xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
      xhr.setRequestHeader("Content-type", "application/json");
      xhr.send(JSON.stringify({companyName: "Sd Software Bv", vatNumber: "BE0716.943.826"}));
    });
  }

  function logout() {
    userManager.signoutRedirect();
  }
})();
