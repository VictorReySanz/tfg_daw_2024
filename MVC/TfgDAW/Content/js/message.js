
document.addEventListener("DOMContentLoaded", function () {
    var messageBox = document.getElementById("messageBox");
    if (messageBox.textContent.trim() !== "") {
        messageBox.classList.add("show");
        setTimeout(function () {
            messageBox.classList.remove("show");
        }, 4000); // Ocultar después de 4 segundos
    }
});