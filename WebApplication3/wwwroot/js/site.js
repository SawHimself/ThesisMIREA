"use strict";

(function () {
    // переключатели механизмов защиты
    document.querySelectorAll('input[type="checkbox"][data-api-endpoint]')
        .forEach(function (checkbox) {
            checkbox.addEventListener("change", function () {
                const url  = `/api/admin/${this.dataset.apiEndpoint}`;
                const body = JSON.stringify({ checked: this.checked });

                fetch(url, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body
                }).then(r => {
                    if (r.ok) {
                        if (this.id === "UseCors") {
                            document.getElementById("rebootRequiredMessage")
                                .style.display = "inline-block";
                        }
                    } else {
                        console.error(`Ошибка ${r.status}: ${r.statusText}`);
                    }
                });
            });
        });

    // сворачивание описаний
    document.querySelectorAll(".toggle-description")
        .forEach(function (btn) {
            btn.addEventListener("click", function () {
                const desc = this.nextElementSibling;
                desc.classList.toggle("show");
                this.textContent = desc.classList.contains("show") ? "▲" : "▼";
            });
        });
})();