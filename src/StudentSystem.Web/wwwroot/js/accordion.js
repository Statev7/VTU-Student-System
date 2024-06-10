const collapsePrefix = '#collapse-';
const accordionPrefix = 'collapse-';

document.addEventListener("DOMContentLoaded", function () {
    const fragment = window.location.hash;
    if (fragment) {
        const targetElement = document.querySelector(fragment);
        if (targetElement && targetElement.classList.contains("accordion-collapse")) {
            const accordionButton = targetElement.previousElementSibling.querySelector(".accordion-button");
            if (accordionButton) {
                accordionButton.classList.remove("collapsed");
                targetElement.classList.add("show");

                const lessonId = fragment.replace(collapsePrefix, '');
                sendAjaxRequest(lessonId);
            }
        }
    }

    document.querySelectorAll('.accordion-button').forEach(function (button) {
        button.addEventListener('click', function () {

            if (button.classList.contains("collapsed")) {
                return;
            }

            const lessonId = button.getAttribute('data-bs-target').replace(collapsePrefix, '');

            sendAjaxRequest(lessonId);
        });
    });

    async function sendAjaxRequest(id) {
        const request = await fetch("/LessonsApi/" + id);
        const response = await request.text();

        const accordion = document.getElementById(accordionPrefix + id);

        if (accordion !== null) {
            accordion.innerHTML = response;
        }
    }
});
