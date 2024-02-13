// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//Toggle Forget Password

const togglePassword = document.querySelector("#togglePassword");
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

