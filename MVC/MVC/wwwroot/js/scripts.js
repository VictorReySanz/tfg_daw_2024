const verMenu = document.getElementById('verMenu');
const menu = document.getElementById('menu');

verMenu.addEventListener('change', function () {
    if (this.checked) {
        menu.style.display = 'block';
        menu.classList.add('aparecer-animacion');
    } else {
        menu.classList.remove('aparecer-animacion');
        menu.style.display = 'none';
    }
});
