let ServerIp="https://localhost:44349";


function empty(e) {
    switch (e.trim()) {
      case "":
      case 0:
      case "0":
      case null:
      case false:
      case typeof(e) == "undefined":
        return true;
      default:
        return false;
    }
  }

  function getRandomInt(max) {
    return Math.floor(Math.random() * max);
  }

  async function postData(url = '', data = {},token='') {
    
    if(token!=''){
      
      var oReq = new XMLHttpRequest();
      oReq.setRequestHeader('Authorization','Token ' + token);
      oReq.open("POST", url, true);
      oReq.onload = function(oEvent) {
        if (oReq.status == 200) {
          console.log("ok");
        } else {
          console.log("no");
        }
      };

      oReq.send(data);
    }

    const response = await fetch(url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
       body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    return response.json(); // parses JSON response into native JavaScript objects
}
function json(url) {
  var json = null;
  $.ajax({
    'async': false,
    'global': false,
    'url': url,
    'dataType': "json",
    'success': function(data) {
      json = data;
    }
  });
  return json.data;
}

let userList = json(ServerIp +"/api/users/getall");
