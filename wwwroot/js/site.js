﻿function ShowHideMenu() {
    var menu = document.getElementById("menu");
    var mainContent = document.getElementById("main-content");
    if (menu.style.display === "none") {
        menu.style.display = "block";
        mainContent.style.width = "67%";
    } else {
        menu.style.display = "none";
        mainContent.style.width = "100%";
    }
};