document.addEventListener("DOMContentLoaded", function () {
    var messageBox = document.getElementById("messageBox");
    if (messageBox.textContent.trim() !== "") {
        messageBox.classList.add("show");
    }
});