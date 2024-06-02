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

// Desplegar menu Admin
const adminDesplegar = document.getElementById('adminDesplegar');
const menuAdmin = document.getElementById('menuAdmin');

adminDesplegar.addEventListener('change', function () {
    if (this.checked) {
        menuAdmin.style.display = 'block';
        menuAdmin.classList.add('aparecerAdmin-animacion');
    } else {
        menuAdmin.classList.remove('aparecerAdmin-animacion');
        menuAdmin.style.display = 'none';
    }
});


// Desplegar menu CV
const cvDesplegar = document.getElementById('cvDesplegar');
const menuCV = document.getElementById('menuCV');

cvDesplegar.addEventListener('change', function () {
    if (this.checked) {
        menuCV.style.display = 'block';
        menuCV.classList.add('aparecerCV-animacion');
    } else {
        menuCV.classList.remove('aparecerCV-animacion');
        menuCV.style.display = 'none';
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
