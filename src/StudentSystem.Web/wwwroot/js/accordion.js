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

                const id = fragment.replace(collapsePrefix, '');
                sendAjaxRequest(id);
            }
        }
    }

    document.querySelectorAll('.accordion-button').forEach(function (button) {
        button.addEventListener('click', function () {

            if (button.classList.contains("collapsed")) {
                return;
            }

            const id = button.getAttribute('data-bs-target').replace(collapsePrefix, '');

            sendAjaxRequest(id, button);
        });
    });

    async function sendAjaxRequest(id, button = null) {

        const href = window.location.href;
        let apiLink = '';

        if (href.includes('Trainings/Details')) {
            apiLink = '/LessonsApi/';
        }
        else if (href.includes('Students/Profile/Trainings')) {
            apiLink = '/CoursesApi/';
            button.style.color = "#F5A425";
        }

        const request = await fetch(apiLink + id);
        const response = await request.text();

        const accordion = document.getElementById(accordionPrefix + id);

        if (accordion !== null) {
            accordion.innerHTML = response;
        }
    }
});
