if(sessionStorage.getItem("login")=="true"){

    window.location.href = "index.html";
}
document.addEventListener("DOMContentLoaded", function(){
    let email = document.getElementById("email");
    let password = document.getElementById("pass");
    let loginBtn = document.getElementById("loginBtn");
    loginBtn.onclick = ()=>{
        
        let _data = {
            email: email.value,
            password: password.value
          }
        postData(ServerIp + "/api/auth/login", _data)
            .then(data => {
                if (data.token != "" && data.expiration != "") {
                    sessionStorage.setItem("login", "true");
                    sessionStorage.setItem("token", data.token);
                    sessionStorage.setItem("mail", _data.email);
                    window.location.href = "index.html";
                }
            });
        


        

    }
    



  

});