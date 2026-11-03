document.querySelectorAll("form").forEach(form => {
    form.addEventListener("submit", () => {
        sessionStorage.setItem("scrollPos", window.scrollY);
    });
});

window.addEventListener("DOMContentLoaded", () => {
    const pos = sessionStorage.getItem("scrollPos");
    window.scrollTo(0, Number(pos));
    sessionStorage.removeItem("scrollPos");
});