 if(sessionStorage.getItem("login")=="true"){

    window.location.href = "index.html";
}
document.addEventListener("DOMContentLoaded", function(){
    
    document.getElementById("registerBtn").onclick = ()=>{
        let name = document.getElementById("name");
        let lastname = document.getElementById("lastname");
        let mail = document.getElementById("mail");
        let pass = document.getElementById("pass");
        let passCorrection = document.getElementById("passCorrection");

        if(pass.value == passCorrection.value)
        {

            let _data = {
                firstName:name.value,
                lastName:lastname.value,
                email: mail.value,
                password: pass.value
            }

            postData(ServerIp + "/api/auth/register", _data)
                .then(data => {
                    if (data.token != "" && data.expiration != "") {
                        sessionStorage.setItem("login", "true");
                        sessionStorage.setItem("token", data.token);
                        sessionStorage.setItem("mail", _data.email);
                        window.location.href = "index.html";
                    }
                });
              
              
        }
        else{
            alert("şifreler uyuşmuyor.");
        }
        
    }
});