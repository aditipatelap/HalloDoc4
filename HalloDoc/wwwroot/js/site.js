// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Toggle Forget Password

/*const togglePassword = document.querySelector("#togglePassword");
const password = document.querySelector("#password");
togglePassword.addEventListener("click", () => {
    // Toggle the type attribute using
    // getAttribure() method
    const type =
        password.getAttribute("type") === "password" ? "text" : "password";
    password.setAttribute("type", type);
    // Toggle the eye and bi-eye icon
    this.classList.toggle("bi-eye");
});
function myFunction() {
    var element = document.body;
    element.classList.toggle("dark-mode");
}
document.getElementById('formFile').addEventListener('change', function () {
    var fileName = this.files[0].name;
    document.getElementById('file-name').textContent = fileName;
})*/;
/****dark mode/****/
const btn1 = document.querySelector(".btn-toggle");
const prefersDarkScheme = window.matchMedia("(prefers-color-scheme: dark)");

const currentTheme = localStorage.getItem("theme");
if (currentTheme == "dark") {
    document.body.classList.toggle("dark-theme");
}
else if (currentTheme == "light") {
    document.body.classList.toggle("light-theme");
}

btn1.addEventListener("click", function () {
    if (prefersDarkScheme.matches) {
        document.body.classList.toggle("light-theme");
        var theme = document.body.classList.contains("light-theme") ? "light" : "dark";
    }
    else {
        document.body.classList.toggle("dark-theme");
        var theme = document.body.classList.contains("dark-theme") ? "dark" : "light";
    }
    localStorage.setItem("theme", theme);
});
//<script>
//    const phoneInputField = document.querySelectorAll("input[type='tel']");
//    for (var i = 0; i < phoneInputField.length; i++) {
//        const phoneInput = window.intlTelInput(phoneInputField[i], {
//        utilsScript:
//    "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
//        });
//    }
//    $(window).on('load', function () {
//        $('#myModal').modal('show');
//    });
//</script>
//<script>

//    (function () {
//        'use strict'

//            // Fetch all the forms we want to apply custom Bootstrap validation styles to
//            var forms = document.querySelectorAll('.needs-validation')

//    // Loop over them and prevent submission
//    Array.prototype.slice.call(forms)
//    .forEach(function (form) {
//        form.addEventListener('submit', function (event) {
//            if (!form.checkValidity()) {
//                event.preventDefault()
//                event.stopPropagation()
//            }

//            form.classList.add('was-validated')
//        }, false)
//    })
//        })()

//</script>