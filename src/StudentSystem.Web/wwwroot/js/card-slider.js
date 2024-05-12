document.getElementsByClassName('owl-stage-outer')[0].style.height = "305px";
document.getElementsByClassName('owl-stage')[0].style.width = "4438px";
const cards = document.getElementsByClassName('owl-item');

for (let index = 0; index < cards.length; index++) {
    cards[index].style.width = "340px";
}