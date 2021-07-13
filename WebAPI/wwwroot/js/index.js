function add() {
  document.getElementById("upload").click();
}
function logout() {
  sessionStorage.setItem("login", "false");
  window.location.href = "index.html";
}
if (sessionStorage.getItem("login") == "true") {
  if (sessionStorage.getItem("userId") == null) {
    for (index in userList) {
      if (userList[index].email == sessionStorage.getItem("mail")) {
        sessionStorage.setItem("userId", userList[index].userId);
      }
    }
  }
} else {
  window.location.href = "login.html";
}

function submitForm() {
  document.getElementById("loading").style.display="flex";
  var fd = new FormData();
  var files = $("#upload")[0].files;

  // Check file selected or not
  if (files.length > 0) {
    fd.append("Image", files[0]);
    fd.append("userId", sessionStorage.getItem("userId"));

    $.ajax({
      url: ServerIp + "/api/userimages/add",
      type: "post",
      data: fd,
      contentType: false,
      processData: false,
      success: function (response) {
        if (response != 0) {
          window.location.reload();
        } else {
          alert("dosya yüklenemedi.");
          document.getElementById("loading").style.display="none";
        }
      },
    });
  } else {
    alert("Lütfen bir dosya seçin.");
    document.getElementById("loading").style.display="none";
  }
}
let profileImages = [];
document.addEventListener("DOMContentLoaded", function () {

  $.getJSON(ServerIp + "/api/userimages/getall", function (json) {
    for (key in json.data) {
      if (json.data[key].profileImage != null) {
        profileImages.push({
          id: json.data[key].userId,
          url: json.data[key].profileImage,
        });
      }
    }

    for (key in json.data.reverse()) {
      if (json.data[key].imagePath != null) {
        var name;

        for (index in userList) {
          if (userList[index].userId == json.data[key].userId) {
            name = userList[index].firstName + " " + userList[index].lastName;

            break;
          }
        }

        var pp = null;

        for (index in profileImages) {
          if (profileImages[index].id == json.data[key].userId) {
            pp = profileImages[index].url;
            break;
          }
        }

        if (pp == null) {
          let rnd = getRandomInt(2);
          if (rnd) {
            pp = "/img/profilew.svg";
          } else {
            pp = "/img/profilem.svg";
          }
        }
        

        document.getElementById("posts").innerHTML +=
          '<div class="post"><div class="info"><img src="' +
          ServerIp +
          pp +
          '" alt="profile picture" class="pp"><p class="name">' +
          name +
          '</p></div><div class="content"><img src="' +
          ServerIp +
          json.data[key].imagePath +
          '" alt=""></div></div>';
      }
    }
    document.getElementById("loading").style.display="none";
  });

  document.getElementById("search").onclick = () => {
    var results = "";
    let query = document.getElementById("query").value.trim();
    if (!empty(query)) {
      for (key in userList) {
        fullName = userList[key].firstName + " " + userList[key].lastName;
        let pp = null;
        if (fullName.toLowerCase().search(query) != -1) {
          
          for (index in profileImages) {
            if (profileImages[index].id == userList[key].userId) {
              pp = profileImages[index].url;
              break;
            }
          }
          if (pp == null) {
            let rnd = getRandomInt(2);
            if (rnd) {
              pp = "/img/profilew.svg";
            } else {
              pp = "/img/profilem.svg";
            }
          }
          results +=
            '<div class="user"><img src="' +
            ServerIp +
            pp +
            '" alt="" class="pp"><span class="name">' +
            userList[key].firstName +
            " " +
            userList[key].lastName +
            "</span></div>";
        }
      }
      document.getElementById("results").innerHTML = results;
    } else {
      document.getElementById("results").innerHTML = results;
    }
  };
  document.getElementById("query").addEventListener("keyup", function (event) {
    // Number 13 is the "Enter" key on the keyboard
    if (event.keyCode === 13) {
      document.getElementById("search").click();
    }
  });
});
