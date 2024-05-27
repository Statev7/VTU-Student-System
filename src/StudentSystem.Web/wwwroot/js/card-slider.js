const owlStageOuter = document.getElementsByClassName('owl-stage-outer')[0];

if (owlStageOuter && owlStageOuter.style) {
    owlStageOuter.style.height = "305px";
}

const owlStage = document.getElementsByClassName('owl-stage')[0];

if (owlStage && owlStage.style) {
    owlStage.style.width = "4438px";
}

const cards = document.getElementsByClassName('owl-item');

for (let index = 0; index < cards.length; index++) {
    cards[index].style.width = "340px";
}