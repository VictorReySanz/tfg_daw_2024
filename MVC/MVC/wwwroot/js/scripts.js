// Desplegar menu
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

// Desplegar filtro
const verFiltro = document.getElementById('verFiltro');
const filtro = document.getElementById('filtro');

verFiltro.addEventListener('change', function () {
    if (this.checked) {
        filtro.style.display = 'block';
        filtro.classList.add('aparecerFiltro-animacion');
    } else {
        filtro.classList.remove('aparecerFiltro-animacion');
        filtro.style.display = 'none';
    }
});

// Desplegar menu de cuenta
const verMenuCuenta = document.getElementById('verMenuCuenta');
const menuCuenta = document.getElementById('menuCuenta');

verMenuCuenta.addEventListener('change', function () {
    if (this.checked) {
        menuCuenta.style.display = 'block';
        menuCuenta.classList.add('aparecerCuenta-animacion');
    } else {
        menuCuenta.classList.remove('aparecerCuenta-animacion');
        menuCuenta.style.display = 'none';
    }
});