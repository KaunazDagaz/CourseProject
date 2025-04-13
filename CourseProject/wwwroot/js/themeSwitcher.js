document.addEventListener("DOMContentLoaded", function () {
    const themeSwitcher = document.getElementById("theme-switcher");
    const allTables = document.querySelectorAll("table");
    function updateTableStyle(theme) {
        allTables.forEach(table => {
            if (theme === "solar") {
                table.classList.add("table-dark");
            } else {
                table.classList.remove("table-dark");
            }
        });
    }
    const themeLink = document.getElementById("bootstrap-theme");
    const savedTheme = localStorage.getItem("theme") || "zephyr";
    themeLink.href = `/css/themes/theme-${savedTheme}.min.css`;
    themeSwitcher.value = savedTheme;
    updateTableStyle(savedTheme);

    themeSwitcher.addEventListener("change", () => {
        const selectedTheme = themeSwitcher.value;
        updateTableStyle(selectedTheme);
        themeLink.href = `/css/themes/theme-${selectedTheme}.min.css`;
        localStorage.setItem("theme", selectedTheme);
    });
});